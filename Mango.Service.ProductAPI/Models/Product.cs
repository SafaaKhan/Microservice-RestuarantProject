﻿using System.ComponentModel.DataAnnotations;

namespace Mango.Service.ProductAPI.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Range(1,1000)]
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;


    }
}
