using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Services;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService productService;

    public ProductsController(IProductService productService)
    {
        this.productService = productService;
    }

    // GET: api/products
    [HttpGet]
    public IActionResult GetProducts()
    {
        var products = productService.GetAll();
        
        if (!products.Any())
        {
            return NotFound(new { message = "Products not found" });
        }
        
        return Ok(products);
    }

    // GET: api/products/1
    [HttpGet("{id}")]
    public IActionResult GetProduct(int id)
    {
        var product = productService.GetById(id);
        
        if (product == null)
        {
            return NotFound(new { message = $"Product with ID {id} not found" });
        }
        
        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public IActionResult CreateProduct([FromBody] Product product)
    {
        var createdProduct = productService.Create(product);
        
        if (createdProduct == null)
        {
            return BadRequest(new { message = "Invalid product data" });
        }
        
        return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
    }

    // PUT: api/products/1
    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, [FromBody] Product product)
    {
        var updatedProduct = productService.Update(id, product);
        
        if (updatedProduct == null)
        {
            return NotFound(new { message = $"Product with ID {id} not found or invalid data" });
        }
        
        return Ok(updatedProduct);
    }

    // DELETE: api/products/1
    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var deleted = productService.Delete(id);
        
        if (!deleted)
        {
            return NotFound(new { message = $"Product with ID {id} not found" });
        }
        
        return NoContent();
    }
}