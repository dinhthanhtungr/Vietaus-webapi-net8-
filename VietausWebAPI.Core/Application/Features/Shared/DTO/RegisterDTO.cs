using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VietausWebAPI.Core.Application.Features.Shared.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "UserName can't be blank.")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email can't be blank.")]
        [EmailAddress(ErrorMessage = "please input email with form.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@vietaus\.com$", ErrorMessage ="Email need have domain @vietaus.com")]
        [Remote(action: "IsEmailAvaiable", controller: "Account")]
        public string Email { get; set; } = string.Empty;
        //[Required(ErrorMessage = "Phone can't be blank.")]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "Phone number should contain digits only")]
        //[Remote(action: "IsEmailAvaiable", controller: "Account")]
        //public string Phone { get; set; } = string.Empty ;
        [Required(ErrorMessage = "PersonName can't be blank.")]
        public string PersonName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password can't be blank.")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Confirm Password can't be blank.")]
        [Compare("Password", ErrorMessage = "Confirm Password not correct.")] 
        public string ConfirmPassword { get; set; } = string.Empty;

        public Guid? EmployeeId { get; set; }
    }
}
