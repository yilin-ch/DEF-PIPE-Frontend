using DataCloud.PipelineDesigner.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.Services.Interfaces
{
    public interface IPublicRepoService
    {
        Task<List<PublicRepo>> Search(string search);
        public Task AddRepo(string user, string workflowName);

        Task RemoveRepo(string user, string WorkflowName);

        public Task<Template> GetPublicRepo(string user, string workflowName);
    }
}
