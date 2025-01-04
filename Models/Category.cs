using System.ComponentModel.DataAnnotations;

namespace Kaalcharakk.Models
{
    public class Category
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
