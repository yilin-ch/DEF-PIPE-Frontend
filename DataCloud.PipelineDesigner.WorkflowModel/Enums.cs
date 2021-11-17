using System;
using System.Collections.Generic;
using System.Text;

namespace DataCloud.PipelineDesigner.WorkflowModel
{
    public enum WorkflowElementType
    { 
        Action = 0,
        Control = 1
    }

    public enum WorkflowControlType
    { 
        Loop = 0,
        Parallel = 1,
        Switch = 2
    }

    public enum WorkflowParamType
    {
        Number = 0,
        Text = 1
    }

    public enum WorkflowCommunicationMedium
    { 
        MessageQueue = 0,
        DistributedFileSystem = 1,
        WebServices = 2
    }

    public enum WorkflowDataSetFormat
    { 
        CSV = 0,
        TSV = 1,
        JSON = 2,
        Zip = 3,
        NoSQLDb = 4,
        GraphDb = 5
    }
}
