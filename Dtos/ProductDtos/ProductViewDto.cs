using Kaalcharakk.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Kaalcharakk.Dtos.ProductDtos
{
    public class ProductViewDto
    {

        public int ProductId { get; set; }

        public string Category { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public string Color { get; set; }

        public int Stock { get; set; }

        public bool IsActive { get; set; } = true;

    }

}