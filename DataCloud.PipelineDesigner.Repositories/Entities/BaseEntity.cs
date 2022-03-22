using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace DataCloud.PipelineDesigner.Repositories.Entities
{
    public class BaseEntity
    {
        [Key, Column(Order = 0)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Workflow { get; set; }
        public string Owner { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy{ get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }


        public BaseEntity()
        {

        }
    }
}
