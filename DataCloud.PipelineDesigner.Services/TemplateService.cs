using DataCloud.PipelineDesigner.CanvasModel;
using DataCloud.PipelineDesigner.Repositories.Models;
using DataCloud.PipelineDesigner.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.Repositories.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly IMongoCollection<BsonDocument> _templatePost;
        private readonly IMongoCollection<Template> _template;
        private readonly IMongoCollection<User> _user;
        private readonly IMongoCollection<BsonDocument> _userPost;
        public TemplateService(IDatabaseSettings settings)
        {
            MongoService mongo = new();

            var db = mongo.GetClient().GetDatabase(settings.DatabaseName);

            _template = db.GetCollection<Template>(settings.TemplateCollectionName);
            _templatePost = db.GetCollection<BsonDocument>(settings.TemplateCollectionName, new MongoCollectionSettings { WriteConcern = WriteConcern.Acknowledged });
            _user = db.GetCollection<User>(settings.UserCollectionName);
            _userPost = db.GetCollection<BsonDocument>(settings.UserCollectionName, new MongoCollectionSettings { WriteConcern = WriteConcern.Acknowledged });
        }

        public Task AddOrUpdateTemplateAsync(Template template)
        {

            string jsonString = JsonConvert.SerializeObject(template);

            BsonDocument document = BsonSerializer.Deserialize<BsonDocument>(jsonString);

            // This fix the issue when updating
            document.Remove("_id");

            return _templatePost.ReplaceOneAsync(
            Builders<BsonDocument>.Filter.Eq("id", template.Id),
            document,
            new ReplaceOptions { IsUpsert = true });
        }


        public Task<DeleteResult> DeleteTemplate(string id)
        {
            return _template.DeleteOneAsync(t => t.Id == id);
        }

        public Task<List<Template>> GetTemplatesAsync()
        {
            return _template.Find(_ => true).ToListAsync();
        }

    }


}
