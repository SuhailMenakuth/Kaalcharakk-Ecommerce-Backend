﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kaalcharakk.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price {  get; set; }
        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string Color {  get; set; }

        [Required]
        public int Stock {  get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Offer { get; set; } = 0;
        public DateTime? OfferStartingDate { get; set; } = DateTime.UtcNow;
       
        public DateTime? OfferEndingDate {  get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        public Category Category { get; set; }

    }
}