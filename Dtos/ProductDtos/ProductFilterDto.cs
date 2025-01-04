namespace Kaalcharakk.Dtos.ProductDtos
{
    public class ProductFilterDto
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Category { get; set; }
        public string? Color { get; set; }
    }
}
