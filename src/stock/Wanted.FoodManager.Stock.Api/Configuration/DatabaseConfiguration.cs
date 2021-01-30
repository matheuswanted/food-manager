using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Reflection;
using Wanted.FoodManager.Stock.Domain;

namespace Wanted.FoodManager.Stock.Api.Configuration
{
    public class DatabaseSettings
    {
        public string Connection { get; set; }
        public string Database { get; set; }
    }

    public static class DatabaseConfiguration
    {
        public static void AddDatabase(this IServiceCollection services, IConfiguration config)
        {
            var settings = new DatabaseSettings();
            config.GetSection("DatabaseSettings").Bind(settings);

            var client = new MongoClient(settings.Connection);
            var database = client.GetDatabase(settings.Database);

            services.AddSingleton<IMongoClient>(client);
            services.AddSingleton<IMongoDatabase>(database);
            ConfigureConventions();
        }

        private static void ConfigureConventions()
        {
            BsonClassMap.RegisterClassMap<Entity>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
            });

            var convetions = new ConventionPack()
            {
                new CamelCaseElementNameConvention(),
                new EnumRepresentationConvention(BsonType.String),
                new IgnoreIfNullConvention(true),
            };

            ConventionRegistry.Register("standard", convetions, _ => true);
        }

        public static void AddCollections(this IServiceCollection services, Type baseType)
        {
            var assembly = Assembly.GetAssembly(baseType);
            var entities = assembly.GetTypes().Where(t => t.IsAssignableTo(baseType));
            var addCollection = typeof(DatabaseConfiguration).GetMethod(nameof(AddCollection), BindingFlags.Public | BindingFlags.Static);

            foreach (var type in entities)
            {
                var toInvoke = addCollection.MakeGenericMethod(type);

                toInvoke.Invoke(null, new[] { services });
            }
        }

        public static void AddCollection<T>(IServiceCollection services)
        {
            services.AddSingleton(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<T>(CamelCase(typeof(T).Name)));
        }

        private static string CamelCase(string word)
        {
            return char.ToLower(word[0]) + word[1..];
        }
    }
}
