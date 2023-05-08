using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AppForVaccine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text; // for class Encoding
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace AppForVaccine.Controllers
{
  
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _db = new ApplicationDbContext();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

      
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        private Random _randdom = new Random();
       public string  GenerateCode()
        {
            return _randdom.Next(0, 9999).ToString("D4");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
          
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var phn = _db.Users.Where(x => x.Email == model.Email).FirstOrDefault();
            var code = _db.SmsCodes.Where(x => x.Email == model.Email).FirstOrDefault();

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {

                case SignInStatus.Success:
                   
                    var smsCode = new SmsCode()
                    { Code = GenerateCode(), Email=phn.Email,Todday =DateTime.Today };
                    _db.SmsCodes.Add(smsCode);
                    _db.SaveChanges();

                    System.Environment.SetEnvironmentVariable("TWILIO_ACCOUNT_SID", "ACed98ad93dd2d2469ae4527f1df18ce4d");
                    System.Environment.SetEnvironmentVariable("TWILIO_AUTH_TOKEN", "7bbab09f3f6c35192515a96d66603f6c");

                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;



                    string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
                    string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");


                    TwilioClient.Init(accountSid, authToken);

                    //var message = MessageResource.Create(
                   //     body: smsCode.Code+ "  שלום, קוד האימות שלך הוא",
                  //      from: new Twilio.Types.PhoneNumber("+13203226247"),
                  //      to: new Twilio.Types.PhoneNumber(phn.PhoneNumber)
                  //  );

                    var snd = new SmsEntity()
                     {

                         content = "Verify code " + smsCode.Code + "for one one day login",
                         Mobile = phn.PhoneNumber,
                         sender = "Vaccine Team"
                     };
                    SmsSenderPoint smsSenderPoint = new SmsSenderPoint();
                    smsSenderPoint.GetAPIReponse(snd);
                    return RedirectToActionPermanent("SmsCode", "Account");
                   

                   
                //return RedirectToLocal(returnUrl);                
                default:
                    ModelState.AddModelError("CustomError", "Invalid Username or Password.");
                    return View(model);

            }
        }

       

        [AllowAnonymous]
        public ActionResult SmsCode()
        {           
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SmsCode(SmsCodeiewModel model)
        {
            var user = User.Identity.Name;
            var usr = _db.Users.Where(x => x.Email == user).FirstOrDefault();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = _db.SmsCodes.Where(x => x.Code ==model.Code && x.Email == usr.Email && x.Todday == DateTime.Today).FirstOrDefault();
            if (result ==null)
            {
                ModelState.AddModelError("CustomError", "Invalid Verification code");
                return View(model);
            }
            else
            {
                if (usr.UserTypes == UserType.Admin)
                {
                    Session["username"] = usr.Name;
                    Session["id"] = usr.Id;
                    return RedirectToActionPermanent("Index", "ManageAccount");
                }
                else
                {
                    Session["username"] = usr.Name;
                    Session["id"] = usr.Id;
                    return RedirectToActionPermanent("IndexOfTherapist", "ManageAccount");
                }
               
            }
           
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

       

        // POST: /Account/LogOff
        //[HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }


}