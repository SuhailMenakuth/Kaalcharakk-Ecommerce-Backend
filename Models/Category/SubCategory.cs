using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kaalcharakk.Models.Category
{
    public class SubCategory
    {

        [Key]
        public int SubCategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        [ForeignKey("ParentCategory")]
        public int ParentCategoryId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }
        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool IsDeleted { get; set; }


        public ParentCategory ParentCategory { get; set; }  // navigation prooperty 

    }
}
