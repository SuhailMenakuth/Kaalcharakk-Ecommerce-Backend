using System.ComponentModel.DataAnnotations;

namespace Kaalcharakk.Models.Category
{
    public class ParentCategory
    {
        [Key]
        public int ParentCategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsDelete { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }

    }
}
