using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Mairala.Areas.Admin.ViewModels
{
    public class UpdateEmployeeVm
    {
        [Required]
        [MinLength(3, ErrorMessage = "Minimum length must be at least 3")]
        [MaxLength(128, ErrorMessage = "Maximum length must be 128")]
        public string Name { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Minimum length must be at least 3")]
        [MaxLength(128, ErrorMessage = "Maximum length must be 128")]
        public string Surname { get; set; }
        //
        public string TwLink { get; set; }
        public string FbLink { get; set; }
        public string LinLink { get; set; }
        //Relational
        [Required]
        public int PositionId { get; set; }
        //Photo
        public string ImageUrl { get; set; }
        public IFormFile? Photo { get; set; }

        public SelectList? Positions { get; set; }
    }
}
