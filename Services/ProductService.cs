using ProductApi.Models;
using ProductApi.DTOs;
using ProductApi.Repositories;

namespace ProductApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository productRepository;

    public ProductService(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<List<ProductResponseDto>> GetAllAsync()
    {
        var products = await productRepository.GetAllAsync();
        return products.Select(MapToResponseDto).ToList();

    }

    public async Task<ProductResponseDto?> GetByIdAsync(int id)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product == null)
            return null;

        return MapToResponseDto(product);
    }

    public async Task<ProductResponseDto?> CreateAsync(CreateProductDto dto)
    {
        // 1. Validar que el producto no sea null
        if (dto == null)
        {
            return null;  // Retornamos null si es null
        }

        // 2. Validar nombre
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return null;  // Retornamos null si el nombre está vacío
        }

        // 3. Validar precio
        if (dto.Price <= 0)
        {
            return null;  // Retornamos null si el precio es inválido
        }

        // 4. Validar stock
        if (dto.Stock < 0)
        {
            return null;  // Retornamos null si el stock es negativo
        }

        // 5. Validar duplicado
        var allProducts = await productRepository.GetAllAsync();
        var existingProduct = allProducts.FirstOrDefault(p =>
            p.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase));

        if (existingProduct != null)
        {
            return null;  // Retornamos null si ya existe
        }

        // 6. Crear producto
        var createdProduct = await productRepository.CreateAsync(new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            Stock = dto.Stock
        });
        if (createdProduct == null)
        {
            return null;
        }

        return MapToResponseDto(createdProduct);
    }

    public async Task<ProductResponseDto?> UpdateAsync(int id, UpdateProductDto dto)
    {
        // 1. Validar ID
        if (id <= 0)
        {
            return null;
        }

        // 2. Validar que el producto no sea null
        if (dto == null)
        {
            return null;
        }

        // 3. Validar nombre
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return null;
        }

        // 4. Validar precio
        if (dto.Price <= 0)
        {
            return null;
        }

        // 5. Validar stock
        if (dto.Stock < 0)
        {
            return null;
        }

        // 6. Validar duplicado (excluyendo el producto actual)
        var allProducts = await productRepository.GetAllAsync();
        var existingProduct = allProducts.FirstOrDefault(p =>
            p.Id != id && p.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase));

        if (existingProduct != null)
        {
            return null;  // Retornamos null si ya existe otro con el mismo nombre
        }

        // 7. Actualizar producto
        var updatedProduct = await productRepository.UpdateAsync(id, new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            Stock = dto.Stock
        });
        // ✅ Validar que el producto fue actualizado correctamente
        if (updatedProduct == null)
        {
            return null;
        }

        return MapToResponseDto(updatedProduct);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
        {
            return false;
        }

        return await productRepository.DeleteAsync(id);
    }

    private static ProductResponseDto MapToResponseDto(Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock
        };

    }
}