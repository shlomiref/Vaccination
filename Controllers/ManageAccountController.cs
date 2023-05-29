using AppForVaccine.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace AppForVaccine.Controllers
{
    [Authorize]
    public class ManageAccountController : Controller
    {
        private const string Vaccinated = "חוסן";
        private const string Not_Vaccinated = "לא חוסן";
        private const string Age0To10 = "0 - 10";
        private ApplicationDbContext _db = new ApplicationDbContext();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageAccountController()
        {
        }

        public ManageAccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

        // GET: ManageAccount
        [HttpGet]
        public ActionResult Index()
        {
            var countersContainer = new CountersContainer();
            countersContainer.Users = _db.Users.Count();
            countersContainer.Therapist = _db.Therapists.Count();
            countersContainer.Clinics = _db.CityLists.Count();
            countersContainer.Patients = _db.Patients.Count();
            return View(countersContainer);
        }
        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //public ActionResult LogOff()
        //{
        //    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        //    return RedirectToAction("Login", "Account");
        //}
        public  ActionResult IndexOfTherapist()
        {
            return View();
        }
        [HttpGet]
        public ActionResult VaccineReminder(int? patientid)
        {
            DateTime filterD = DateTime.Now.AddDays(7); // Get patient that are due in 7 days a
            if (patientid != null)
            {
                var PatientData = _db.PatientVaccinations.Where(x => x.PatientId == patientid && x.Status == false && x.scheduledDate <= filterD && x.ReminderSent==false).ToList();
                var vReminderlist = new List<VaccinationReminderModel>();

                foreach (var item in PatientData)
                {
                    var vr = new VaccinationReminderModel()
                    {
                        VaccineName = item.VaccineName,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        PatientId = item.PatientId,
                        Status = item.Status,
                        PatientNumber = item.PatientNumber,
                        PatientVaccinatedId = item.PatientVaccinatedId,
                        scheduledDate = item.scheduledDate.Value.ToString("MM/dd/yyyy")
                    };
                    vReminderlist.Add(vr);
                }
                return View(vReminderlist);
            }
            else
            {
                var PatientData = _db.PatientVaccinations.Where(x => x.Status == false && x.scheduledDate <= filterD ).ToList();
                var vReminderlist = new List<VaccinationReminderModel>();

                foreach (var item in PatientData)
                {
                    var vr = new VaccinationReminderModel()
                    {
                        VaccineName = item.VaccineName,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        PatientId = item.PatientId,
                        Status = item.Status,
                        PatientNumber = item.PatientNumber,
                        ReminderSent = item.ReminderSent.Value,
                        PatientVaccinatedId = item.PatientVaccinatedId,
                        scheduledDate = item.scheduledDate.Value.ToString("MM/dd/yyyy")
                    };
                    vReminderlist.Add(vr);
                }
                return View(vReminderlist);
            }
        }

        public void TaskReminder()
        {
            DateTime filterD = DateTime.Now.AddDays(7);
          
                var PatientData = _db.PatientVaccinations.Where(x => x.Status == false && x.scheduledDate <= filterD).ToList();
                var vReminderlist = new List<VaccinationReminderModel>();
                
                foreach (var item in PatientData)
                {
                    var vr = new VaccinationReminderModel()
                    {
                        VaccineName = item.VaccineName,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        PatientId = item.PatientId,
                        Status = item.Status,
                        PatientVaccinatedId = item.PatientVaccinatedId,
                        scheduledDate = item.scheduledDate.Value.ToString("MM/dd/yyyy"),

                    };

                var phone = _db.Patients.Where(x => x.PatientId == item.PatientId).FirstOrDefault();


                var snd = new SmsEntity()
                    {
                    content = "Vaccine for " + item.VaccineName + "is due to take place",
                    Mobile = phone.Phone,
                        //sender = "",
                    };
                    SmsSenderPoint smsSenderPoint = new SmsSenderPoint();
                    smsSenderPoint.GetAPIReponse(snd);
               }    
        }

        
        public ActionResult GetPatient( string patientid)
        {
            //if (patientid !=null)
            //{
                var PatientData = _db.Patients.Where(x => x.PatientNumber == patientid).FirstOrDefault();
                return View(PatientData);
            //}
            //var patientd = _db.Patients.Where(x =>x.PatientId == 1).FirstOrDefault();
         
           // return View(patientd);
        }

        [HttpGet]
        public ActionResult GetVaccination(string patientid) 
        {
            //if (patientid != null)
            //{
                var PatientData = _db.PatientVaccinations.Where(x => x.PatientNumber == patientid).ToList();
                var vList = new List<PatientVaccinationModelList>();
                foreach (var item in PatientData)
                {

                    bool dat = false;
                    if (item.VaccinationDate == null) { dat = true; }
                    bool thera = false;
                    if (item.Therapist == null) { thera = true; }
                    //b = a != null ? a : b;
                    var nuser = new PatientVaccinationModelList()
                    {
                        PatientVaccinatedId = item.PatientVaccinatedId,
                        VaccineName = item.VaccineName,
                        Status = item.Status,
                        Therapist = thera ? null : item.Therapist,
                        VaccinatedDate = dat ? null : item.VaccinationDate.Value.ToString("MM/dd/yyyy"),
                        NextVaccineDate = item.NextVaccinaDate.Value.ToString("MM/dd/yyyy")
                    };
                    vList.Add(nuser);
                }
                if (PatientData.Count == 0)
                {
                var vaccinationModel1 = new PatientVaccination()
                {
                   
                    PatList = vList
                };
                return View(vaccinationModel1);
            }
                var vaccinationModel = new PatientVaccination()
                {
                    PatientNumber= PatientData.Select(x => x.PatientNumber).First(),
                    PatientId = PatientData.Select(x => x.PatientId).First(),
                    FirstName = PatientData.Select(x =>x.FirstName).First(),
                    LastName = PatientData.Select(x => x.LastName).First(),
                    UniqueId = PatientData.Select(x => x.UniqueId).First(),
                    PatList = vList
                };
                return View(vaccinationModel);
            //}
            //else
            //{
            //    var PatientData = _db.PatientVaccinations.Where(x => x.PatientId == 0).ToList();
            //    var vList = new List<PatientVaccinationModelList>();
            //    foreach (var item in PatientData)
            //    {
            //        bool dat = false;
            //        if (item.VaccinationDate == null) { dat = true;   }
            //        bool thera = false;
            //        if (item.Therapist == null) { thera = true; }
            //        //b = a != null ? a : b;
            //        var nuser = new PatientVaccinationModelList()
            //        {
            //            PatientVaccinatedId = item.PatientVaccinatedId,
            //            VaccineName = item.VaccineName,
            //            Status = item.Status,
            //            Therapist = thera ? null : item.Therapist,
            //            VaccinatedDate =  dat ? null : item.VaccinationDate.Value.ToString("MM/dd/yyyy"),
            //            NextVaccineDate = item.NextVaccinaDate.Value.ToString("MM/dd/yyyy")
            //        };
            //        vList.Add(nuser);
            //    }
            //    var vaccinationModel = new PatientVaccinationModel()
            //    {
            //        PatientId = 1,
            //        FirstName = PatientData.Select(x => x.FirstName).First(),
            //        LastName = PatientData.Select(x => x.LastName).First(),
            //        PatList = vList
            //    };
            //    return View(vaccinationModel);
            //}
            //var patientd = _db.PatientVaccinations.Where(x => x.PatientId == 1).ToList();
            //return View(patientd);
        }
       [HttpGet]
        public ActionResult VaccinatePatient1(int id)
        {
            ViewBag.City = new SelectList(_db.CityLists, "City", "City");
            var PatientData = _db.PatientVaccinations.Where(x => x.PatientVaccinatedId == id).FirstOrDefault();
            if (PatientData == null)
            {
                return HttpNotFound();
            }           
            var editVacci = new vaccineP()
            {
                FirstName = PatientData.FirstName,
                LastName = PatientData.LastName,
                Status = PatientData.Status,
                vaccineName = PatientData.VaccineName,
                PatientVaccinatedId = PatientData.PatientVaccinatedId,
                PatientId = PatientData.PatientId,
                PatientNumber = PatientData.PatientNumber
                
                
            };
            ViewBag.message = "upadated Sucessfully";
            return View(editVacci);
        }
        [HttpPost]
        public ActionResult VaccinatePatient1(vaccineP model)
        {
            ViewBag.City = new SelectList(_db.CityLists, "City", "City");
            var th = User.Identity.Name;
            var usr = _db.Users.Where(x => x.Email == th).FirstOrDefault();
            model.Therapist = usr.Name;
            var PatientData = _db.PatientVaccinations.Where(x => x.PatientVaccinatedId == model.PatientVaccinatedId).FirstOrDefault();
            if (PatientData == null) throw new Exception("Patient does not exist");
            var entity = model.EditModel(PatientData, model);
            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToRoute(new
            {
                controller = "ManageAccount",
                action = "GetVaccination",
                patientid = PatientData.PatientNumber
            });
        }
        //[HttpPost]
        //public JsonResult VaccinatePatient(vaccineP vaccineP)
        //{
        //    var PatientData = _db.PatientVaccinations.Where(x => x.PatientId == vaccineP.PatientId).FirstOrDefault();
        //    if (PatientData == null) throw new Exception("Patient does not exist");
        //    //var entity = model.Edit(isExist, model);
        //    //_db.Entry(entity).State = EntityState.Modified;
        //    //_db.SaveChanges();
        //    return Json(vaccineP, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public ActionResult GetTherapist()
        {
            var TherapistList = _db.Therapists.ToList();
           
            return View(TherapistList); 
        }

        [HttpGet]
        public ActionResult UserList()
        {
            var user = _db.Users.ToList();
            var userList = new List<MyUserModel>();
            foreach (var item in user)
            {
                var nuser = new MyUserModel()
                {
                    id= item.Id,
                    Name = item.Name,
                    Email= item.Email,
                    PhoneNumber = item.PhoneNumber,
                    Status = item.Status,
                    UserTypes = item.UserTypes
                };
                userList.Add(nuser);
            }
            int numberOfRecords = userList.Count;
            return View(userList);

        }


        public ActionResult Register()
        {
            return View();
        }       
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber, UserTypes = model.UserTypes, Name = model.Name };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var ifCreated = await _db.Users.Where(x => x.Email == model.Email).FirstOrDefaultAsync();
                    if (ifCreated != null)
                    {                       
                            var th = new Therapist()
                            {
                                Email = ifCreated.Email,
                                Name = model.Name,
                                Phone = model.PhoneNumber,
                                Clinic = "Test"
                            };
                            _db.Therapists.Add(th);
                            _db.SaveChanges();
                       
                    }
                    return RedirectToAction("UserList", "ManageAccount");
                   // return PartialView("_Success");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);

          
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangesPasswordViewModel model) //Password#123
        {
            if (ModelState.IsValid)
            {
                var usr = User.Identity.Name;
                var userid = _db.Users.Where(x => x.Email == usr).FirstOrDefault();
                var result = UserManager.ChangePassword(userid.Id, model.CurrentPassword, model.Password);
                if (result.Succeeded)
                {                   
                    return RedirectToAction("Login", "Account");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        

        public ActionResult GetVaccinationRecord()
        {
            var vaccinePat = _db.PatientVaccinations.ToList();           
            return View(vaccinePat);
        }
        public ActionResult GetVaccine()
        {
            var vaccinePat = _db.Vaccinations.ToList();
            return View(vaccinePat);
        }
        public ActionResult GetCity()
        {
            var cities = _db.CityLists.ToList();
            return View(cities);
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public ActionResult AddPatient()
        {
            ViewBag.City = new SelectList(_db.CityLists, "City", "City");
            return View();
        }
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPatient(PatientModel model)
        {
            ViewBag.City = new SelectList(_db.CityLists, "City", "City");
            if (ModelState.IsValid)
            {

                var result = await _db.Patients.Where(x => x.Email==model.Email).FirstOrDefaultAsync();
                try
                {
                    if (result == null)
                    {
                        var valRecord = new Patient()
                        {
                            Email = model.Email,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            City = model.City,
                            Birthday = model.Birthday,
                            Phone = model.Phone,
                            Address = model.Address,
                            DateTime = DateTime.Now,
                            UniqueId = RandomString(8),
                            PatientNumber = model.PatientNumber,


                        };
                        _db.Patients.Add(valRecord);
                        _db.SaveChanges();                    //return RedirectToAction("UserList", "ManageAccount");

                        var ifCreated = await _db.Patients.Where(x => x.Email == model.Email).FirstOrDefaultAsync();
                        if (ifCreated != null)
                        {
                            var vaccine = _db.Vaccinations.ToList();
                            foreach (var item in vaccine)
                            {                               
                                int month = Convert.ToInt32(ifCreated.Birthday.ToString("MM")) + item.Month.Value;
                                DateTime nextAppointment = ifCreated.Birthday.AddMonths(month);
                                var valpat = new PatientVaccination()
                                {
                                    PatientId = ifCreated.PatientId,
                                    FirstName = model.FirstName,
                                    LastName = model.LastName,
                                    VaccineName = item.VaccineName,
                                    Status = false,
                                    UniqueId = ifCreated.UniqueId,
                                    City = ifCreated.City,
                                    ReminderSent = false,
                                    NextVaccinaDate= nextAppointment,
                                    PatientNumber = ifCreated.PatientNumber,
                                   // NextVaccination = "2023 - 02 - 02  " + item.Month,
                                    scheduledDate = Convert.ToDateTime(item.ValidityOfVaccine)

                                };
                                _db.PatientVaccinations.Add(valpat);
                                _db.SaveChanges();
                            }
                        }
                        return RedirectToAction("GetVaccination", "ManageAccount");
                    }
                    return PartialView("_patientexist"); ;
                    AddErrors(result);
                }
                catch (Exception ex) 
                {

                    throw;
                }
                
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult AddVaccine()
        {
            return View();
        }
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddVaccine(Vaccination model)
        {
            if (ModelState.IsValid)
            {

                var result = await _db.Vaccinations.Where(x => x.VaccineName == model.VaccineName).FirstOrDefaultAsync();
                if (result == null)
                {
                    var addval = new Vaccination()
                    {
                        VaccineName = model.VaccineName,
                        Month = model.Month,
                        ValidityOfVaccine = model.ValidityOfVaccine,
                        Manufacturer = model.Manufacturer
                    };
                    var pati = await _db.Patients.ToListAsync();                  
                       
                        foreach (var item in pati)
                         {
                        var valpat = new PatientVaccination()
                        {
                            PatientId = item.PatientId,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            VaccineName = model.VaccineName,
                            Status = false,
                            PatientNumber = item.PatientNumber,
                            // NextVaccination = "2023 - 02 - 02  " + item.Month,
                            scheduledDate = Convert.ToDateTime(model.ValidityOfVaccine),

                            };
                            _db.PatientVaccinations.Add(valpat);
                            _db.SaveChanges();
                        }
                 
                    _db.Vaccinations.Add(addval);
                    _db.SaveChanges();
                    return RedirectToAction("GetVaccine", "ManageAccount");
                }
                return PartialView("_failtoAddvaccine"); ;
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult AddCity()
        {
            return View();
        }        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCity(CityList model)
        {
            if (ModelState.IsValid)
            {

                var result = await _db.CityLists.Where(x => x.City == model.City).FirstOrDefaultAsync();
                if (result == null)
                {                    
                        _db.CityLists.Add(model);
                        _db.SaveChanges();                   
                }

              

            }
            return RedirectToAction("GetCity", "ManageAccount");
        }

        public ActionResult ChangePasswordAdmin()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePasswordAdmin(ChangesPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usr = User.Identity.Name;
                var userid = _db.Users.Where(x => x.Email == usr).FirstOrDefault();
                var result = UserManager.ChangePassword(userid.Id, model.CurrentPassword, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        private void AddErrors(object result)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult Edit(string id)        
        {
            if (id == null)
            {
                return View();
            }
            var edit = _db.Users.Where(x => x.Id == id).FirstOrDefault();
            if (edit == null)
            {
                return HttpNotFound();
            }
            var edituser = new MyUserModel()
            {
                id= edit.Id,
                Name = edit.Name,
                Email= edit.Email,
                PhoneNumber = edit.PhoneNumber,
                Status = edit.Status,
                UserTypes = edit.UserTypes
            };
            ViewBag.message = "Login Sucessfully";
            return View(edituser);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(MyUserModel model)
        {
            if (ModelState.IsValid)
            {
                var userUpdate = new ApplicationUser {Id= model.id, UserName = model.Email, Email = model.Email,Status = model.Status, PhoneNumber = model.PhoneNumber, UserTypes = model.UserTypes, Name = model.Name };
                var isExist = _db.Users.Find(model.id);
                if (isExist == null) throw new Exception("Patient does not exist");
                var entity = model.Edit(isExist, model);
                _db.Entry(entity).State = EntityState.Modified;             
                _db.SaveChanges();
                return RedirectToAction("UserList");
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Profile(string id)
        {
            if (id == null)
            {
                return View();
            }
            var edit = _db.Users.Where(x => x.Id == id).FirstOrDefault();
            if (edit == null)
            {
                return HttpNotFound();
            }
            var edituser = new MyUserModel()
            {
                id = edit.Id,
                Name = edit.Name,
                Email = edit.Email,
                PhoneNumber = edit.PhoneNumber,
                Status = edit.Status,
                UserTypes = edit.UserTypes
            };
            ViewBag.message = "Login Sucessfully";
            return View(edituser);
        }

        [HttpGet]
        public ActionResult EVaccination(int id)
        {
            if (id == null)
            {
                return View();
            }
            var edit = _db.Vaccinations.Where(x => x.vaccineId == id).FirstOrDefault();
            if (edit == null)
            {
                return HttpNotFound();
            }
            var edituser = new VaccinationModel()
            {
                vaccineId = id,
                VaccineName = edit.VaccineName,
                ValidityOfVaccine = edit.ValidityOfVaccine,
                Manufacturer = edit.Manufacturer,
                Month = edit.Month.Value
            };
           /// ViewBag.message = "Log Sucessfully";
            return View(edituser);
        }
        [HttpPost]
        public async Task<ActionResult> EVaccination(VaccinationModel model)
        {
            if (ModelState.IsValid)
            {
                var userUpdate = new Vaccination
                {
                    vaccineId =model.vaccineId,
                    VaccineName = model.VaccineName,
                    ValidityOfVaccine = model.ValidityOfVaccine,
                    Manufacturer = model.Manufacturer,
                };
                var isExist = _db.Vaccinations.Find(model.vaccineId);
                if (isExist == null) throw new Exception("Vaccine does not exist");
                var entity = model.Edit(isExist, model);
                _db.Entry(entity).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("GetVaccine");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ECity(int id)
        {
            if (id == null)
            {
                return View();
            }
            var edit = _db.CityLists.Where(x => x.Id == id).FirstOrDefault();
            if (edit == null)
            {
                return HttpNotFound();
            }
            var edituser = new cityModel()
            {
                Id = id,
                City = edit.City
            };           
            return View(edituser);
        }
        [HttpPost]
        public async Task<ActionResult> ECity(cityModel model)
        {
            if (ModelState.IsValid)
            {                
                var isExist = _db.CityLists.Find(model.Id);
                if (isExist == null) throw new Exception("City does not exist");
                var entity = model.Edit(isExist, model);
                _db.Entry(entity).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("GetVaccine");
            }
            return View(model);
        }





        [HttpPost]
        public async Task<ActionResult> Profile(MyUserModel model)
        {
            if (ModelState.IsValid)
            {
                var userUpdate = new ApplicationUser { Id = model.id, UserName = model.Email, Email = model.Email, Status = model.Status, PhoneNumber = model.PhoneNumber, UserTypes = model.UserTypes, Name = model.Name };
                var isExist = _db.Users.Find(model.id);
                if (isExist == null) throw new Exception("Patient does not exist");
                var entity = model.Edit(isExist, model);
                _db.Entry(entity).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("IndexOfTherapist");
            }
            return View(model);
        }
        public ActionResult AdminProfile(string id)
        {
            if (id == null)
            {
                return View();
            }
            var edit = _db.Users.Where(x => x.Id == id).FirstOrDefault();
            if (edit == null)
            {
                return HttpNotFound();
            }
            var edituser = new MyUserModel()
            {
                id = edit.Id,
                Name = edit.Name,
                Email = edit.Email,
                PhoneNumber = edit.PhoneNumber,
                Status = edit.Status,
                UserTypes = edit.UserTypes
            };
            ViewBag.message = "Login Sucessfully";
            return View(edituser);
        }
        [HttpPost]
        public async Task<ActionResult> AdminProfile(MyUserModel model)
        {
            if (ModelState.IsValid)
            {
                var userUpdate = new ApplicationUser { Id = model.id, UserName = model.Email, Email = model.Email, Status = model.Status, PhoneNumber = model.PhoneNumber, UserTypes = model.UserTypes, Name = model.Name };
                var isExist = _db.Users.Find(model.id);
                if (isExist == null) throw new Exception("Patient does not exist");
                var entity = model.Edit(isExist, model);
                _db.Entry(entity).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("IndexOfTherapist");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Report(string VaccineName, string Age, string City, string Status)
        {
            //ViewBag.Vaccinations = _db.Vaccinations.ToList();
            bool st = false;
            dynamic ageRange = null;
            int Arange = 0;
            ViewBag.Vaccinations = new SelectList(_db.Vaccinations, "VaccineName", "VaccineName");
            ViewBag.Age = new SelectList(new List<string> { Age0To10, "11 - 20", "21 - 30", "31 - 40" , "41  and Above" });
            //ViewBag.City = new SelectList(new List<string> { "Tel Viv", "Jerusalem", "Haifa" });
            ViewBag.City = new SelectList(_db.CityLists, "City", "City");
            ViewBag.Status = new SelectList(new List<string> { Vaccinated, Not_Vaccinated });
            if (VaccineName != null)
            {
                if (Status == Vaccinated) { st = true; }else if(Status == Not_Vaccinated) { st = false; }

                var PatientData = _db.PatientVaccinations.Where(x =>x.Status == st && x.VaccineName == VaccineName && x.City==City).ToList();
                var vReminderlist = new List<ReportModel>();
                var vReprtAfterAge = new List<ReportModel>();
                foreach (var item in PatientData)
                { 
                    var pti = _db.Patients.Where(x => x.PatientId == item.PatientId).FirstOrDefault(); 
                    var today = DateTime.Today;
                    var age = today.Year - pti.Birthday.Year;
                    if (pti.Birthday.Date > today.AddYears(-age)) age--;
                    var vr = new ReportModel()
                    {
                        VaccineName = item.VaccineName,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        PatientId = item.PatientId,
                        Status = item.Status,
                        City = item.City,
                       Age  = age,
                       PatientNumber = item.PatientNumber
                    };
                    vReminderlist.Add(vr);
                }                
                if (Age == Age0To10) { vReprtAfterAge = vReminderlist.Where(p => p.Age >= 0 && p.Age <= 10).ToList(); }
                if (Age == "11 - 20") { vReprtAfterAge = vReminderlist.Where(p => p.Age >= 11 && p.Age <= 20).ToList(); }
                if (Age == "21 - 30") { vReprtAfterAge = vReminderlist.Where(p => p.Age >= 21 && p.Age <= 30).ToList(); }
                if (Age == "31 - 40") { vReprtAfterAge = vReminderlist.Where(p => p.Age >= 31 && p.Age <= 40).ToList(); }
                if (Age == "41  and Above") { vReprtAfterAge = vReminderlist.Where(p => p.Age >= 41).ToList(); }


                return View(vReprtAfterAge);
            }
            else
            {
                var PatientData = _db.PatientVaccinations.Take(100).ToList();
                var vReminderlist = new List<ReportModel>();

                foreach (var item in PatientData)
                {
                    var pti = _db.Patients.Where(x => x.PatientId == item.PatientId).FirstOrDefault();

                    var today = DateTime.Today;
                    var age = today.Year - pti.Birthday.Year;
                    if (pti.Birthday.Date > today.AddYears(-age)) age--;
                    var vr = new ReportModel()
                    {
                        VaccineName = item.VaccineName,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        PatientId = item.PatientId,
                        Status = item.Status,
                        City = pti.City,
                        Age = age,
                        PatientNumber = item.PatientNumber
                    };
                    vReminderlist.Add(vr);
                }               
                return View(vReminderlist);
            }
        }

        [HttpGet]
        public ActionResult ReportDownload(string VaccineName, string Age, string City, string Status)
        {
            bool st = false;
            dynamic ageRange = null;
            int Arange = 0;
            ViewBag.Vaccinations = new SelectList(_db.Vaccinations, "VaccineName", "VaccineName");
            ViewBag.Age = new SelectList(new List<string> { Age0To10, "11 - 20", "21 - 30", "31 - 40", "41  and Above" });
            //ViewBag.City = new SelectList(new List<string> { "Tel Viv", "Jerusalem", "Haifa" });
            ViewBag.City = new SelectList(_db.CityLists, "City", "City");
            ViewBag.Status = new SelectList(new List<string> { Vaccinated, Not_Vaccinated });
            if (Status == Vaccinated) { st = true; } else if (Status == Not_Vaccinated) { st = false; }
            var PatientData = _db.PatientVaccinations.Where(x => x.Status == st && x.VaccineName == VaccineName && x.City == City).ToList();
            if (VaccineName != null)
            {
                var vReminderlist = new List<ReportModel>();
                var vReprtAfterAge = new List<ReportModel>();
                foreach (var item in PatientData)
                {
                    var pti = _db.Patients.Where(x => x.PatientId == item.PatientId).FirstOrDefault();
                    var today = DateTime.Today;
                    var age = today.Year - pti.Birthday.Year;
                    if (pti.Birthday.Date > today.AddYears(-age)) age--;
                    var vr = new ReportModel()
                    {
                        VaccineName = item.VaccineName,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        PatientId = item.PatientId,
                        Status = item.Status,
                        City = item.City,
                        Age = age,
                        PatientNumber = item.PatientNumber
                    };
                    vReminderlist.Add(vr);
                }
                if (Age == Age0To10) { vReprtAfterAge = vReminderlist.Where(p => p.Age >= 0 && p.Age <= 10).ToList(); }
                if (Age == "11 - 20") { vReprtAfterAge = vReminderlist.Where(p => p.Age >= 11 && p.Age <= 20).ToList(); }
                if (Age == "21 - 30") { vReprtAfterAge = vReminderlist.Where(p => p.Age >= 21 && p.Age <= 30).ToList(); }
                if (Age == "31 - 40") { vReprtAfterAge = vReminderlist.Where(p => p.Age >= 31 && p.Age <= 40).ToList(); }
                if (Age == "41  and Above") { vReprtAfterAge = vReminderlist.Where(p => p.Age >= 41).ToList(); }

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].LoadFromCollection(vReprtAfterAge, true);
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //here i have set filname as Students.xlsx
                    Response.AddHeader("content-disposition", "attachment;  filename=Doctor_VaccineReport.xlsx");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }

                return View(vReprtAfterAge);
            }
            else
            {
                PatientData = _db.PatientVaccinations.Take(100).ToList();
                var vReminderlist = new List<ReportModel>();

                foreach (var item in PatientData)
                {
                    var pti = _db.Patients.Where(x => x.PatientId == item.PatientId).FirstOrDefault();

                    var today = DateTime.Today;
                    var age = today.Year - pti.Birthday.Year;
                    if (pti.Birthday.Date > today.AddYears(-age)) age--;
                    var vr = new ReportModel()
                    {
                        VaccineName = item.VaccineName,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        PatientId = item.PatientId,
                        Status = item.Status,
                        City = pti.City,
                        Age = age,
                        PatientNumber = item.PatientNumber
                    };
                    vReminderlist.Add(vr);
                }
                return View(vReminderlist);
            }
        }
        public void DownloadExcel( string UniqueId)
        {
            if (UniqueId != null)
            {
                var PatientData = _db.PatientVaccinations.Where(x => x.UniqueId == UniqueId).ToList();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage Ep = new ExcelPackage();
                ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
                Sheet.Cells["A1"].Value = "FirstName";
                Sheet.Cells["B1"].Value = "LastName";
                Sheet.Cells["C1"].Value = "VaccineName";
                Sheet.Cells["D1"].Value = "PatientId";
                Sheet.Cells["E1"].Value = "Status";
                Sheet.Cells["F1"].Value = "Therapist";              
                int row = 2;
                foreach (var item in PatientData)
                {
                    dynamic stTrue = null;                 
                    if (item.Status == true)
                    {
                        stTrue = "Vaccinated";
                    }
                    else
                    {
                        stTrue = Not_Vaccinated;
                    }

                    Sheet.Cells[string.Format("A{0}", row)].Value = item.FirstName;
                    Sheet.Cells[string.Format("B{0}", row)].Value = item.LastName;
                    Sheet.Cells[string.Format("C{0}", row)].Value = item.VaccineName;
                    Sheet.Cells[string.Format("D{0}", row)].Value = item.PatientId;
                    Sheet.Cells[string.Format("E{0}", row)].Value = stTrue;
                    Sheet.Cells[string.Format("F{0}", row)].Value = item.Therapist;                    
                    row++;
                }


                Sheet.Cells["A:AZ"].AutoFitColumns();
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "Report.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();
            }
        }

        public void DownloadReportExcel(string VaccineName, string Age, string City, string Status)
        {
            if (VaccineName != null)
            {
                var PatientData = _db.PatientVaccinations.Where(x => x.VaccineName == VaccineName).ToList();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage Ep = new ExcelPackage();
                ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
                Sheet.Cells["A1"].Value = "FirstName";
                Sheet.Cells["B1"].Value = "LastName";
                Sheet.Cells["C1"].Value = "VaccineName";
                Sheet.Cells["D1"].Value = "PatientId";
                Sheet.Cells["E1"].Value = "Status";
                Sheet.Cells["F1"].Value = "Therapist";
                int row = 2;
                foreach (var item in PatientData)
                {
                    dynamic stTrue = null;
                    if (item.Status == true)
                    {
                        stTrue = "Vaccinated";
                    }
                    else
                    {
                        stTrue = Not_Vaccinated;
                    }

                    Sheet.Cells[string.Format("A{0}", row)].Value = item.FirstName;
                    Sheet.Cells[string.Format("B{0}", row)].Value = item.LastName;
                    Sheet.Cells[string.Format("C{0}", row)].Value = item.VaccineName;
                    Sheet.Cells[string.Format("D{0}", row)].Value = item.PatientId;
                    Sheet.Cells[string.Format("E{0}", row)].Value = stTrue;
                    Sheet.Cells[string.Format("F{0}", row)].Value = item.Therapist;
                    row++;
                }


                Sheet.Cells["A:AZ"].AutoFitColumns();
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "Report.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();
            }
        }
        public void ExportListUsingEPPlus(string VaccineName, string Age, string City, string Status)
        {
            
                     
            
        }

    }
}