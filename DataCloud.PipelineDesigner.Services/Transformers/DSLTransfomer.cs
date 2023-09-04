using DataCloud.PipelineDesigner.Services.Interfaces;
using DataCloud.PipelineDesigner.WorkflowModel;
using DataCloud.PipelineDesigner.WorkflowModel.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataCloud.PipelineDesigner.Services
{
    public class DSLTransfomer: IDSLTransformer
    {
        public Dsl Dsl { get; set; }
        public DSLTransfomer(Dsl dsl)
        {            
            this.Dsl = dsl;
        }


        public string Transform()
        {
            return this.Dsl.ToString();
        }

        public static Dsl Parse(string dsl)
        {
            Workflow workflow = new Workflow();
            try
            {
                var tokens = DslTokenizer.TryTokenize(dsl);
                if (!tokens.HasValue)
                {
                    throw new Exception(tokens.ErrorMessage.ToString());
                }
                else if (!DslParser.TryParse(tokens.Value, out Dsl expr, out var error, out var errorPosition))
                {
                    throw new Exception(error.ToString());
                }
                else
                {
                    return expr;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }


        private string Identation(int level)
        {
            return new String('\t', level + 1);
        }
    }
}
