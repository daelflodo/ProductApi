using ProductApi.Models;

namespace ProductApi.Repositories;

public interface IProductRepository
{
    List<Product> GetAll();

    Product? GetById(int id);

    Product Create(Product product);

    Product? Update(int id, Product product);

    bool Delete(int id);
}