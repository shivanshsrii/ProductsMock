using Microsoft.AspNetCore.Http;

public class ProductCreateDto
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public IFormFile? Image { get; set; }
}
