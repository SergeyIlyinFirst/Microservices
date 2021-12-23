using Catalog.API.Entitites;

namespace Catalog.API.Repositories
{
    public interface IProductReposirory
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(string id);
        Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName);

        Task CreateProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(string id);
    }
}
