using ProductApi.Models;
using ProductApi.Repositories;

namespace ProductApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository productRepository;

    public ProductService(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public List<Product> GetAll()
    {
        return productRepository.GetAll();
    }
    public Product? GetById(int id)
    {
        return productRepository.GetById(id);
    }
    public Product? Create(Product product)
    {
        // 1. Validar que el producto no sea null
        if (product == null)
        {
            return null;  // Retornamos null si es null
        }

        // 2. Validar nombre
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            return null;  // Retornamos null si el nombre está vacío
        }

        // 3. Validar precio
        if (product.Price <= 0)
        {
            return null;  // Retornamos null si el precio es inválido
        }

        // 4. Validar stock
        if (product.Stock < 0)
        {
            return null;  // Retornamos null si el stock es negativo
        }

        // 5. Validar duplicado
        var allProducts = productRepository.GetAll();
        var existingProduct = allProducts.FirstOrDefault(p =>
            p.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase));

        if (existingProduct != null)
        {
            return null;  // Retornamos null si ya existe
        }

        // 6. Crear producto
        return productRepository.Create(product);
    }

    public Product? Update(int id, Product product)
    {
        // 1. Validar ID
        if (id <= 0)
        {
            return null;
        }

        // 2. Validar que el producto no sea null
        if (product == null)
        {
            return null;
        }

        // 3. Validar nombre
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            return null;
        }

        // 4. Validar precio
        if (product.Price <= 0)
        {
            return null;
        }

        // 5. Validar stock
        if (product.Stock < 0)
        {
            return null;
        }

        // 6. Validar duplicado (excluyendo el producto actual)
        var allProducts = productRepository.GetAll();
        var existingProduct = allProducts.FirstOrDefault(p =>
            p.Id != id && p.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase));

        if (existingProduct != null)
        {
            return null;  // Retornamos null si ya existe otro con el mismo nombre
        }

        // 7. Actualizar producto
        return productRepository.Update(id, product);
    }
    public bool Delete(int id)
    {
        if (id <= 0)
        {
            return false;
        }

        return productRepository.Delete(id);
    }
}