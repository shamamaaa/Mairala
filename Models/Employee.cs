using System.ComponentModel.DataAnnotations.Schema;

namespace Mairala.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        //
        public string TwLink { get; set; }
        public string FbLink { get; set; }
        public string LinLink { get; set; }
        //Relational
        public int PositionId { get; set; }
        public Position Position { get; set; }

        //Photo
        public string ImageUrl { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
