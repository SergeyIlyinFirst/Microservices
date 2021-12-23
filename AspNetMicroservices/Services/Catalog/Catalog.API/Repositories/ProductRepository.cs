using Catalog.API.Data;
using Catalog.API.Entitites;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductReposirory
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context
                            .Products
                            .FindAsync(p => true)
                            .Result
                            .ToListAsync();
        }
        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _context
                           .Products
                           .FindAsync(p => p.Id == id)
                           .Result
                           .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

            return await _context
                            .Products
                            .FindAsync(filter)
                            .Result
                            .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

            return await _context
                            .Products
                            .FindAsync(filter)
                            .Result
                            .ToListAsync();
        }

        public async Task CreateProductAsync(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);

            ReplaceOneResult updateResult = await _context
                                        .Products
                                        .ReplaceOneAsync(filter, replacement: product);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Products
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
    }
}
