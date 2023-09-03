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
            testConnection(_client);
        }

        public async void testConnection(MongoClient _client)
        {
            try
            {
                // Try to perform a sample operation, such as listing the collection names
                var database = _client.GetDatabase("mydatabase");
                var collectionNames = await database.ListCollectionNames().ToListAsync();

                // If the operation succeeds without throwing an exception, the connection is successful
                Console.WriteLine("Connected to MongoDB!");
            }
            catch (Exception ex)
            {
                // If an exception is thrown, the connection failed
                Console.WriteLine("Failed to connect to MongoDB: " + ex.Message);
            }
        }

        public MongoClient GetClient()
        {
            return _client;
        }
    }
}
