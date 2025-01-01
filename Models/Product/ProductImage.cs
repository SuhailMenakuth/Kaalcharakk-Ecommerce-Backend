using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kaalcharakk.Models.Product
{
    public class ProductImage
    {
        [Key]
        public int ProductImageId { get; set; }


        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        [StringLength(500)]
        public string ImageUrl { get; set; }

        public Product Product { get; set; }
    }
}
