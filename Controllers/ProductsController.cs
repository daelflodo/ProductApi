using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using System.Text.Json;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> products = new()
    {
        new Product
        {
            Id = 1,
            Name = "Laptop",
            Price = 1200,
            Stock = 10,
        },
        new Product
        {
            Id = 2,
            Name = "mouse",
            Price = 25,
            Stock = 50,
        } 
    };

     // GET: api/products
        [HttpGet]
        public IActionResult GetProducts()
    {
        return Ok(products);
    }

    // GET: api/products/1
    [HttpGet("{id}")]
    public IActionResult GetProduct(int id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);

        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public IActionResult CreateProduct(Product product)
    {

        product.Id = products.Count + 1;
        var json = JsonSerializer.Serialize(product, new JsonSerializerOptions 
        { 
            WriteIndented = true // Para formato bonito
        });
        Console.WriteLine(json);

        products.Add(product);

        return Ok(product);
    }

    // PUT: api/products/1
    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, Product product)
    {
        var _product = products.FirstOrDefault(p => p.Id == id);
        _product.Name = product.Name;
        _product.Price = product.Price;
        _product.Stock = product.Stock;

        return Ok(_product);
    }

    // DELETE: api/products/1
    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);

        products.Remove(product);

        return Ok();
    }

}
