using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kaalcharakk.Models.Product
{
    public class ProductSize
    {
        [Key]
        public int ProductSizeId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        [StringLength(10)]
        public string Size { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Product Product { get; set; }
    }
}
