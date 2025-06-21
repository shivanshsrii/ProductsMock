using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductsMock.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Swagger dependency
builder.Services.AddSwaggerGen();           // Swagger generator

// Add PostgreSQL DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));

var app = builder.Build();

// Use Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable serving static files (for images from wwwroot)
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
