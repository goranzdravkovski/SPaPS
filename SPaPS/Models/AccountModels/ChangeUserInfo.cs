using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SPaPS.Models.AccountModels
{
    public class ChangeUserInfo
    {
        [Required]
        public string? PhoneNumber { get; set; }
        public int ClientTypeId { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string IdNo { get; set; } = null!;
        public int CityId { get; set; }
        public int? CountryId { get; set; }
    }
}
