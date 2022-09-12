using System.ComponentModel;

namespace SPaPS.Models.AccountModels
{
    public class LoginModel
    {
        [DisplayName("Кориснички име")]
        public string Email { get; set; }
        [DisplayName("Лозинка")]
        public string Password { get; set; }
    }
}
