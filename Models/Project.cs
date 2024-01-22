using System.ComponentModel.DataAnnotations.Schema;

namespace Mairala.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        //Photo
        public string ImageUrl { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
