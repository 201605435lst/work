using System;
using Mongo2Go;

namespace SnAbp.TenantManagement.MongoDB
{
    public class MongoDbFixture : IDisposable
    {
        private static readonly MongoDbRunner MongoDbRunner;
        public static readonly string ConnectionString;

        static MongoDbFixture()
        {
            MongoDbRunner = MongoDbRunner.Start();
            ConnectionString = MongoDbRunner.ConnectionString;
        }

        public void Dispose()
        {
            MongoDbRunner?.Dispose();
        }
    }
}
