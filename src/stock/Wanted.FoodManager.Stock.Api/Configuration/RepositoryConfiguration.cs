using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wanted.FoodManager.Stock.Api.Repositories;

namespace Wanted.FoodManager.Stock.Api.Configuration
{
    public static class RepositoryConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<ShoppingListRepository>();
        }
    }
}
