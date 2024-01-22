using System.ComponentModel.DataAnnotations;

namespace Mairala.Areas.Admin.ViewModels
{
    public class RegisterVm
    {
        [Required]
        [MinLength(3,ErrorMessage ="Minimum length must be at least 3")]
        [MaxLength(128, ErrorMessage = "Maximum length must be 128")]
        public string Name { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Minimum length must be at least 3")]
        [MaxLength(128, ErrorMessage = "Maximum length must be 128")]
        public string Surname { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Minimum length must be at least 3")]
        [MaxLength(128, ErrorMessage = "Maximum length must be 128")]
        public string UserName { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "Minimum length must be at least 5")]
        [MaxLength(320, ErrorMessage = "Maximum length must be 320")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }


    }
}
