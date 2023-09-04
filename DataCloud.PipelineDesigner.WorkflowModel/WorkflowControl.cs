using System;
using System.Collections.Generic;
using System.Text;

namespace DataCloud.PipelineDesigner.WorkflowModel
{
    public abstract class WorkflowControl : WorkflowElement
    {
        public string ID { get; set; }
        public Guid? InputDataSetId { get; set; }
        public override WorkflowElementType ElementType => WorkflowElementType.Control;
        public abstract WorkflowControlType ControlType { get; }

        public List<WorkflowElement> Elements { get; set; }
    }

    public class WorkflowLoopControl: WorkflowControl
    {
        public override WorkflowControlType ControlType => WorkflowControlType.Loop;
        public int IterationCount { get; set; }
    }

    public class WorkflowParallelControl: WorkflowControl
    {
        public override WorkflowControlType ControlType => WorkflowControlType.Parallel;
    }

    public class WorkflowSwitchControl : WorkflowControl
    {
        public override WorkflowControlType ControlType => WorkflowControlType.Switch;

        public Dictionary<string, List<WorkflowElement>> SwitchCases { get; set; }

        public WorkflowSwitchControl()
        {
            SwitchCases = new Dictionary<string, List<WorkflowElement>>();
        }
    }
}
