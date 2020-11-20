using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wanted.FoodManager.Stock.Domain;

namespace Wanted.FoodManager.Stock.Api.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        Task<T> Find(string id);
        Task<List<T>> List(int page, int size);
        Task<T> Save(T entity);
    }

    public class GenericRepository<T> : IRepository<T> where T : Entity
    {
        private readonly IMongoCollection<T> _collection;

        public GenericRepository(IMongoCollection<T> collection)
        {
            _collection = collection;
        }

        public async Task<T> Find(string id)
        {
            return await _collection.AsQueryable()
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<T>> List(int page, int size)
        {
            return await _collection.AsQueryable()
                .Skip(page * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<T> Save(T entity)
        {
            if (entity.Id == null)
            {
                await _collection.InsertOneAsync(entity);
            }
            else
            {
                var result = await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq(e => e.Id, entity.Id), entity);
            }
            return entity;
        }
    }

    public class ShoppingListRepository : GenericRepository<ShoppingList>
    {
        public ShoppingListRepository(IMongoCollection<ShoppingList> collection) : base(collection)
        {
        }
    }
}
