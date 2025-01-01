using System.ComponentModel.DataAnnotations;

namespace Kaalcharakk.Dtos.ProductDtos
{
    public class ProductDto
    {
        //public int ProductId { get; set; }
        public int CategoryId { get; set; }

        [Required]
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal price { get; set; }
        public decimal Discount { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public string Color { get; set; }
        //public List<string> Images { get; set; }
        //[Required]
        //public ICollection<ProductSizeDto> ProductSizes { get; set; }
    }
}
