using System;
using System.Collections.Generic;
using System.Text;

namespace DataCloud.PipelineDesigner.Repositories.Entities
{
    internal class BaseEntity
    {
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy{ get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
    }
}
