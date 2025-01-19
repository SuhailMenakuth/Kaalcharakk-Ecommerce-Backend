using System.ComponentModel.DataAnnotations;

namespace Kaalcharakk.Dtos.ProductDtos
{
    public class ProductFilterDto
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }


        [RegularExpression(@"^(Men|Women)$", ErrorMessage = "Category name must be either 'Men' or 'Women'.")]
        public string? CategoryName { get; set; }
        public string? Color { get; set; }
    }
}
