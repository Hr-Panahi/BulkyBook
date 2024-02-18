using System.ComponentModel.DataAnnotations;

namespace BulkyBookWeb.Models
{
    public class Category
    {
        [Key] //data annotation: it will tell entity framework to make sure that id is pk and identity column
        public int Id { get; set; }
        [Required] //it will make sure that Name is non-nullable property
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
