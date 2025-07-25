﻿namespace ProductApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty; // newly added
        public string? ImageUrl { get; set; }
    }
}
