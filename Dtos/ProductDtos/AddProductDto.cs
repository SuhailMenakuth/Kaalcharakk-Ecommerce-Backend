using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Kaalcharakk.Dtos.ProductDtos
{
    public class AddProductDto
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public string Color { get; set; }
        
        public decimal Offer { get; set; } = 0;

        public DateTime? OfferStartingDate { get; set; } = DateTime.UtcNow;

        public DateTime? OfferEndingDate { get; set; } = DateTime.UtcNow;
        

    }
}
