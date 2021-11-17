using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.CanvasModel
{  
    public class CanvasElementProperty
    {
        public string Name { get; set; }

        public string Value { get; set; }
        public CanvasElementPropertyType Type { get; set; }
        public List<string> Options { get; set; }
        public bool? AllowEditing { get; set; }
    }
}
