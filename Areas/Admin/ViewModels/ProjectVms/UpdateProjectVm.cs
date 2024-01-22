using System.ComponentModel.DataAnnotations.Schema;

namespace Mairala.Areas.Admin.ViewModels
{
    public class UpdateProjectVm
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        //Photo
        public string ImageUrl { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
