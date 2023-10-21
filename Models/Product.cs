using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }

        public string ProductImageUrl { get; set; }

        [MaxLength(int.MaxValue)] // Allow for large text in Description
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public DateTime CreateDt { get; set; }
        public DateTime UpdateDt { get; set; }
    }
}