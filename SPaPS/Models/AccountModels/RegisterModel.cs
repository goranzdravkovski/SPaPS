using System.ComponentModel.DataAnnotations;

namespace SPaPS.Models.AccountModels
{
    public class RegisterModel
    {
        [Display(Name = "E-маил")]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int ClientTypeId { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string IdNo { get; set; } = null!;
        public int CityId { get; set; }
        public int? CountryId { get; set; }
        public string? Role { get; set; }
        public int? NoOfEmployees { get; set; }
        public DateTime? DateOfEstablishment { get; set; }

    }
}
