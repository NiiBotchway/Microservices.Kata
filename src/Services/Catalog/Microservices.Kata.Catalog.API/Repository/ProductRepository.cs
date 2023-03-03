using Microservices.Kata.Catalog.API.Data;
using Microservices.Kata.Catalog.API.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microservices.Kata.Catalog.API.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            //return await _context.Products.FindAsync(p => true).GetAwaiter().GetResult().ToListAsync();
            return await _context.Products.FindAsync(FilterDefinition<Product>.Empty).GetAwaiter().GetResult().ToListAsync();
            //.GetAwaiter().GetResult().ToListAsync();
        }

        public async Task<Product> GetProductById(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            return await _context.Products.FindAsync(filter).GetAwaiter().GetResult().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            //FilterDefinition<Product> filter = Builders<Product>.Filter.StringIn(p => p.Name, name);
            FilterDefinition<Product> filter = Builders<Product>.Filter.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(new Regex(name, RegexOptions.IgnoreCase)));

            var prod = await _context.Products.FindAsync<Product>(filter).GetAwaiter().GetResult().ToListAsync();
            return prod;
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string category)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Regex(p => p.Category, new MongoDB.Bson.BsonRegularExpression(new Regex(category, RegexOptions.IgnoreCase)));

            //FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, category);

            return await _context.Products.FindAsync(filter).GetAwaiter().GetResult().ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateRes = await _context
                                    .Products
                                    .ReplaceOneAsync(filter: p => p.Id == product.Id, replacement: product);

            return updateRes.IsAcknowledged && updateRes.IsModifiedCountAvailable && updateRes.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var deleteRes = await _context
                                    .Products
                                    .DeleteOneAsync(filter: p => p.Id == id);

            return deleteRes.IsAcknowledged && deleteRes.DeletedCount > 0;
        }
    }
}
