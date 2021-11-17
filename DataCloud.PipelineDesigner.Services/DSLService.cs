using DataCloud.PipelineDesigner.Services.Interfaces;
using DataCloud.PipelineDesigner.WorkflowModel;
using DataCloud.PipelineDesigner.WorkflowModel.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.Services
{
    public class DSLService: IDSLService
    {
        IDSLTransformer dslTransformer;

        public DSLService()
        {
            //TODO: Update this to use dependency injection
            dslTransformer = new SimpleDSLTransfomer();
        }

        public List<DSLInfo> GetAvailableDSL()
        {
            var dsl = new List<DSLInfo>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    var attrs = type.GetCustomAttributes(typeof(DSLMetadata), false);
                    if (attrs.Length > 0)
                    {
                        var dslAttr = attrs[0] as DSLMetadata;
                        DSLInfo dslInfo = new DSLInfo() { Name = dslAttr.Name, TemplateProperties = new List<string>() };

                        var typeMembers = type.GetMembers();
                        foreach (var typeMember in typeMembers)
                        {
                            var propAttrs = typeMember.GetCustomAttributes(typeof(DSLTemplateProperty), false);
                            foreach (var propAttr in propAttrs)
                            {
                                dslInfo.TemplateProperties.Add((propAttr as DSLTemplateProperty).Name);
                            }
                        }

                        dsl.Add(dslInfo);
                    }
                }
            }

            return dsl;
        }

        public string TransformWorkflowToDSL(Workflow workflow)
        {
            return dslTransformer.Transform(workflow);
        }
    }
}
