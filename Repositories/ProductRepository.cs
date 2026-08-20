using ProductApi.Models;
namespace ProductApi.Repositories;

public class ProductRepository : IProductRepository
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
            Name = "Mouse",
            Price = 25,
            Stock = 50,
        }
    };
    public async Task<List<Product>> GetAllAsync()
    {
        return await Task.FromResult(products);
    }
    public async Task<Product?> GetByIdAsync(int id)
    {
        return await Task.FromResult( products.FirstOrDefault(p => p.Id == id));
    }
    public async Task<Product?> CreateAsync(Product product)
    {
        // ✅ La forma más LINQ y elegante
        product.Id = products.Select(p => p.Id).DefaultIfEmpty(0).Max() + 1;
        products.Add(product);
        return await Task.FromResult(product);
    }
    public async Task<Product?> UpdateAsync(int id, Product product)
    {
        var existingProduct = products.FirstOrDefault(p => p.Id == id);

        if (existingProduct == null)
        {
            return null;
        }

        existingProduct.Name = product.Name;
        existingProduct.Price = product.Price;
        existingProduct.Stock = product.Stock;

        return await Task.FromResult(existingProduct);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return false;
        }

        products.Remove(product);

        return await Task.FromResult(true);
    }
}