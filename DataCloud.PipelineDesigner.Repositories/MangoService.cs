using MongoDB.Driver;
using System;

namespace DataCloud.PipelineDesigner.Repositories
{
    public class MongoService
    {
        private static MongoClient _client;

        public MongoService()
        {
            string connectionString = Environment.GetEnvironmentVariable("MANGO_CONNECTION_STRING");
            _client = new MongoClient(connectionString);
        }

        public MongoClient GetClient()
        {
            return _client;
        }
    }
}
