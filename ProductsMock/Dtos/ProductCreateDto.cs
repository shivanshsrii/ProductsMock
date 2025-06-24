using Microsoft.AspNetCore.Http;

public class ProductCreateDto
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty; // newly added
    public IFormFile? Image { get; set; }
}
