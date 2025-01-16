namespace Kaalcharakk.Dtos.ProductDtos
{
    
        public class UpdateProductDto
        {
            public int CategoryId { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Color { get; set; }
            public int Stock { get; set; }
            public decimal Offer { get; set; }
            public DateTime? OfferStartingDate { get; set; }
            public DateTime? OfferEndingDate { get; set; }
            public bool IsActive { get; set; }
        }

    
}
