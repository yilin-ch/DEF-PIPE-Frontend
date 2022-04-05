using DataCloud.PipelineDesigner.Repositories;
using DataCloud.PipelineDesigner.Repositories.Models;
using DataCloud.PipelineDesigner.Services.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.Services
{
    public class UserService : IUserService
    {

        private readonly IMongoCollection<User> _user;
        public UserService(IDatabaseSettings settings)
        {
            MongoService mongo = new();

            var db = mongo.GetClient().GetDatabase(settings.DatabaseName);

            _user = db.GetCollection<User>(settings.UserCollectionName);
        }


        public Task<List<User>> GetUsersAsync()
        {
            return _user.Find(_ => true).ToListAsync();
        }
    }
}
