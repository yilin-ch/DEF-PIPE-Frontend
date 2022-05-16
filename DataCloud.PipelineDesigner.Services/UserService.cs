using DataCloud.PipelineDesigner.Repositories;
using DataCloud.PipelineDesigner.Repositories.Models;
using DataCloud.PipelineDesigner.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DataCloud.PipelineDesigner.Services
{
    public class UserService : IUserService
    {

        private readonly IMongoCollection<User> _user;
        private readonly IMongoCollection<BsonDocument> _userPost;
        public UserService(IDatabaseSettings settings)
        {
            MongoService mongo = new();

            var db = mongo.GetClient().GetDatabase(settings.DatabaseName);

            _user = db.GetCollection<User>(settings.UserCollectionName);
            _userPost = db.GetCollection<BsonDocument>(settings.UserCollectionName, new MongoCollectionSettings { WriteConcern = WriteConcern.Acknowledged });
        }


        public Task<List<User>> GetUsersAsync()
        {
            return _user.Find(_ => true).ToListAsync();
        }



        public Task<UpdateResult> UpdateRepoAsync(Template template, string user)
        {

            string jsonString = JsonConvert.SerializeObject(template);

            var document = BsonSerializer.Deserialize<BsonDocument>(jsonString);

            return _userPost.UpdateOneAsync(
                    Builders<BsonDocument>.Filter.Eq("Username", user) &
                    Builders<BsonDocument>.Filter.Eq("Templates.id", template.Id),
                    Builders<BsonDocument>.Update.Set("Templates.$", document));
        }

        public Task<UpdateResult> AddRepoAsync(Template template, string user)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            string templateString = JsonConvert.SerializeObject(template.CanvasTemplate, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = contractResolver,
            });
            var templateDoc = JsonConvert.DeserializeObject<dynamic>(templateString);

            var temp = template;
            temp.CanvasTemplate = templateDoc;
            
            string temeString = JsonConvert.SerializeObject(template);
            var document = BsonSerializer.Deserialize<BsonDocument>(temeString);

            return _userPost.UpdateOneAsync(
                    Builders<BsonDocument>.Filter.Eq("Username", user) ,
                    Builders<BsonDocument>.Update.AddToSet("Templates", document));
        }

        public Task<User> GetRepoAsync(string user)
        {
            return _user.Find(u => u.Username == user).FirstAsync();
        }

        public Task<User> DeleteTemplate(string user, string id)
        {
            var update = Builders<User>.Update.PullFilter(u => u.Templates,
                                                t => t.Id == id);
            return _user.FindOneAndUpdateAsync(u => u.Username == user, update);
        }
    }
}
