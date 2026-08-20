using Microsoft.AspNetCore.Mvc;
using ProductApi.DTOs;
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
    public async Task<IActionResult> GetProducts()
    {
        var products = await productService.GetAllAsync();
        
        if (!products.Any())
        {
            return NotFound(new { message = "Products not found" });
        }
        
        return Ok(products);
    }

    // GET: api/products/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await productService.GetByIdAsync(id);
        
        if (product == null)
        {
            return NotFound(new { message = $"Product with ID {id} not found" });
        }
        
        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
    {
        var createdProduct = await productService.CreateAsync(dto);
        
        if (createdProduct == null)
        {
            return BadRequest(new { message = "Invalid product data" });
        }
        
        return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
    }

    // PUT: api/products/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto dto)
    {
        var updatedProduct = await productService.UpdateAsync(id, dto);
        
        if (updatedProduct == null)
        {
            return NotFound(new { message = $"Product with ID {id} not found or invalid data" });
        }
        
        return Ok(updatedProduct);
    }

    // DELETE: api/products/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deleted = await productService.DeleteAsync(id);
        
        if (!deleted)
        {
            return NotFound(new { message = $"Product with ID {id} not found" });
        }
        
        return NoContent();
    }
}