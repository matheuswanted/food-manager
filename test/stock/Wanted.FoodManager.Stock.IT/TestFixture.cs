using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Wanted.FoodManager.Stock.Api;

namespace Wanted.FoodManager.Stock.IT
{
    public class TestFixture
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public GrpcChannel GrpcChannel { get; }

        public TestFixture()
        {
            _factory = new WebApplicationFactory<Startup>();
            var client = _factory.CreateDefaultClient(new ResponseVersionHandler());
            GrpcChannel = GrpcChannel.ForAddress(client.BaseAddress, new GrpcChannelOptions
            {
                HttpClient = client
            });
        }
        public void Dispose()
        {
            _factory.Dispose();
        }
    }
}
