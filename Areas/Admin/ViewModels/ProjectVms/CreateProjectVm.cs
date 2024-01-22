namespace Mairala.Areas.Admin.ViewModels
{
    public class CreateProjectVm
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        //Photo
        public IFormFile Photo { get; set; }
    }
}
