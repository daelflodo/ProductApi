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
    public List<Product> GetAll()
    {
        return products;
    }
    public Product? GetById(int id)
    {
        return products.FirstOrDefault(p => p.Id == id);
    }
    public Product Create(Product product)
    {
        // ✅ La forma más LINQ y elegante
        product.Id = products.Select(p => p.Id).DefaultIfEmpty(0).Max() + 1;
        products.Add(product);
        return product;
    }
    public Product? Update(int id, Product product)
    {
        var existingProduct = products.FirstOrDefault(p => p.Id == id);

        if (existingProduct == null)
        {
            return null;
        }

        existingProduct.Name = product.Name;
        existingProduct.Price = product.Price;
        existingProduct.Stock = product.Stock;

        return existingProduct;
    }

    public bool Delete(int id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return false;
        }

        products.Remove(product);

        return true;
    }
}