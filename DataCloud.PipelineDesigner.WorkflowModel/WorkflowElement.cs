using System;
using System.Collections.Generic;
using System.Text;

namespace DataCloud.PipelineDesigner.WorkflowModel
{
    public abstract class WorkflowElement
    {
        public abstract WorkflowElementType ElementType { get; }
    }
}
