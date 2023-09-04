using DataCloud.PipelineDesigner.Repositories;
using DataCloud.PipelineDesigner.Repositories.Models;
using DataCloud.PipelineDesigner.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.Services
{
    public class PublicRepoService : IPublicRepoService
    {
        private readonly IMongoCollection<User> _user;
        private readonly IMongoCollection<PublicRepo> _publicRepo;

        public PublicRepoService(IDatabaseSettings settings)
        {
            MongoService mongo = new();

            var db = mongo.GetClient().GetDatabase(settings.DatabaseName);

            _publicRepo = db.GetCollection<PublicRepo>(settings.PublicRepoCollectionName);
            _user = db.GetCollection<User>(settings.UserCollectionName);

        }

        public Task<List<PublicRepo>> Search(string search)
        {

            var filterUser = Builders<PublicRepo>.Filter.Regex(p => p.User, new BsonRegularExpression(search, "i"));
            var filterWorkflow = Builders<PublicRepo>.Filter.Regex(p => p.WorkflowName, new BsonRegularExpression(search, "i"));
            var filters = Builders<PublicRepo>.Filter.Or(filterUser, filterWorkflow);


            return _publicRepo.Find(filter: filters).ToListAsync();
        }


        public async Task<Template> GetPublicRepo(string user, string workflowName)
        {

            var filterUser = Builders<PublicRepo>.Filter.Eq(p => p.User, user);
            var filterWorkflow = Builders<Template>.Filter.Eq(p => p.Name, workflowName);


            var repo = await _user.Find(
                Builders<User>.Filter.Eq(u => u.Username, user)).FirstAsync();

            Template template = repo.Templates.Find(t => t.Name == workflowName);


            if ((template is not null) && template.Public)
            {
                return template;
            }
            else
            {
              await this.RemoveRepo(user, workflowName);
              throw new InvalidOperationException("This repo is not available anymore");
            }


            
        }

        public Task AddRepo (string user, string workflowName)
        {

            PublicRepo pr = new PublicRepo{
                User = user,
                WorkflowName = workflowName,
            };

            return _publicRepo
                .ReplaceOneAsync(
                Builders<PublicRepo>.Filter.Eq(p => p.User, pr.User) & 
                Builders<PublicRepo>.Filter.Eq(p => p.WorkflowName, pr.WorkflowName),
                pr,
                new ReplaceOptions { IsUpsert = true });
        }

        public Task RemoveRepo(string user, string WorkflowName)
        {

            return _publicRepo
                 .DeleteManyAsync(t => t.User == user && t.WorkflowName == WorkflowName);
        }

    }
}
