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
}
