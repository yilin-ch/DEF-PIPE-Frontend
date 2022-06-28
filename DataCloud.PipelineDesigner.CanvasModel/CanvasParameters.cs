using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.CanvasModel
{
    public class CanvasParameters
    {
        public string Implementation { get; set; }
        public string Image { get; set; }
        public List<EnvironmentParameter> EnvironmentParameters { get; set; }
        public string ResourceProvider  { get; set; }
        public string StepType  { get; set; }
        public string StepImplementation { get; set; }
    }

    public class EnvironmentParameter
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
