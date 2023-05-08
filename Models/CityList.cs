using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppForVaccine.Models
{
    public class CityList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string City { get; set; }
    }

    public class cityModel
    {
        public int Id { get; set; }
        [Required]
        public string City { get; set; }

        public CityList Edit(CityList entity, cityModel model)
        {
            entity.Id = model.Id;
            entity.City = model.City;
         
            return entity;
        }
    }
}