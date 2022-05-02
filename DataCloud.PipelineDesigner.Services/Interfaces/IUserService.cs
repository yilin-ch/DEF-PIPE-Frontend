using DataCloud.PipelineDesigner.Repositories.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetUsersAsync();
        Task<UpdateResult> UpdateRepoAsync(Template template, string user);
        Task<UpdateResult> AddRepoAsync(Template template, string user);
        Task<User> GetRepoAsync(String user);

        Task<User> DeleteTemplate(String user, String id);
    }
}
