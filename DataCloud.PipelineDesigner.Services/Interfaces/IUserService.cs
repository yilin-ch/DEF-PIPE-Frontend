using DataCloud.PipelineDesigner.Repositories.Models;
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
    }
}
