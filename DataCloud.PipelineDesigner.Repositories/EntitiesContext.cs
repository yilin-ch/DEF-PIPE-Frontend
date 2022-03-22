using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using DataCloud.PipelineDesigner.Core;
using System.Configuration;
using DataCloud.PipelineDesigner.Repositories.Entities;


namespace DataCloud.PipelineDesigner.Repositories
{
    public class EntitiesContext : DbContext
    {

        public EntitiesContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            String connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            optionsBuilder.UseSqlServer(connectionString);
        }

        //entities
        public DbSet<BaseEntity> BaseEntities { get; set; }
    }

}