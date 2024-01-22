using System.ComponentModel.DataAnnotations;

namespace Mairala.Areas.Admin.ViewModels
{
    public class LoginVm
    {
        [Required]
        [MinLength(3, ErrorMessage = "Minimum length must be at least 3")]
        [MaxLength(320, ErrorMessage = "Maximum length must be 320")]
        public string UserNameOrEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemembered { get; set; }
    }
}
