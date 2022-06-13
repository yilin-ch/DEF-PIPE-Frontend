using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using System.Linq;

using TokenParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, object>;
using StringParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, string>;
using KeyvalueParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, System.Collections.Generic.KeyValuePair<string, string>>;
using EnvParamsParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, System.Collections.Generic.Dictionary<string, string>>;
using ExecutionRequParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, DataCloud.PipelineDesigner.WorkflowModel.DSL.ExecutionRequirements>;
using ExecutionRequsParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, DataCloud.PipelineDesigner.WorkflowModel.DSL.ExecutionRequirements[]>;
using StepParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, DataCloud.PipelineDesigner.WorkflowModel.DSL.Step>;
using StepsParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, DataCloud.PipelineDesigner.WorkflowModel.DSL.Step[]>;
using ProviderParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, DataCloud.PipelineDesigner.WorkflowModel.DSL.Provider>;
using PiplineParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, DataCloud.PipelineDesigner.WorkflowModel.DSL.Pipeline>;
using RootParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, DataCloud.PipelineDesigner.WorkflowModel.DSL.Dsl>;
using DataCloud.PipelineDesigner.WorkflowModel.DSL;
using System.Collections.Generic;

namespace DataCloud.PipelineDesigner.Services
{
    public static class DslParser
    {
        

        public static StringParser TextContent =>
            Token.EqualTo(DSlToken.Text)
            .Select(n => n.ToStringValue());

        public static StringParser String =>
        from String in TextContent.Many()
        select string.Join(" ", String);

        public static StringParser PipelineName =>
            from name in TextContent
            select (string)name;

        public static StringParser StepName =>
            from stepName in TextContent.Many()
            select string.Join(" ", stepName);

        public static StringParser StepImplementation =>
            from impl in Token.EqualTo(DSlToken.Implem)
            from stepImpl in TextContent.Many()
            select string.Join(" ", stepImpl);

        public static StringParser StepImage =>
            from image in Token.EqualTo(DSlToken.Image)
            from stepImage in TextContent.Many()
            select string.Join(" ", stepImage);

        public static StringParser StepResource =>
            from rsrc in Token.EqualTo(DSlToken.Resrc)
            from stepRsrc in TextContent.Many()
            select string.Join(" ", stepRsrc);


        public static StringParser StepPrevious =>
            from prev in Token.EqualTo(DSlToken.Prev)
            from setpPrev in TextContent.Many()
            select string.Join(" ", setpPrev);



        public static KeyvalueParser Envs =>
            from value in Token.EqualTo(DSlToken.Text)
            select new KeyValuePair<string, string>(value.ToStringValue().Split("=")[0], value.ToStringValue().Split("=")[1]);

        public static EnvParamsParser StepEnvParam =>
            from parm in Token.EqualTo(DSlToken.EnvParam)
            from start in Token.EqualTo(DSlToken.LBracket)
            from env in Envs.Many()
            from end in Token.EqualTo(DSlToken.RBrracket)
            select env.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);

        public static KeyvalueParser ExecSpec =>
            from value in TextContent.Many()
            select new KeyValuePair<string, string>(value[0], value[1]);

        public static ExecutionRequParser Req =>
            from type in TextContent
            from subtype in TextContent
            from start in Token.EqualTo(DSlToken.LBracket)
            from spec in ExecSpec.Many()
            from end in Token.EqualTo(DSlToken.RBrracket)
            select new ExecutionRequirements { Type = type, SubType = subtype, Requirements = spec.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value)};

        public static ExecutionRequsParser StepExecReq =>
            from exec in Token.EqualTo(DSlToken.ExecRequ)
            from req in Req.Many()
            select req;

        static StepParser Step { get; } =
            from _ in Token.EqualTo(DSlToken.StepSign).Optional()
            from name in StepName
            from implementation in StepImplementation
            from image in StepImage.OptionalOrDefault()
            from env in StepEnvParam.OptionalOrDefault()
            from execRequ in StepExecReq.OptionalOrDefault()
            from resource in StepResource.OptionalOrDefault()
            from previous in StepPrevious.OptionalOrDefault()
            select new Step { Name = name, Implementation = implementation, Image = image, ResourceProvider = resource, Previous = previous, ExecRequirements = execRequ , EnvParams= env };

        static StepsParser Steps { get; } =
            from stepSign in Token.EqualTo(DSlToken.Steps)
            from steps in Step.Many()
            select steps;

        static ProviderParser Provider { get; } =
            from type in Token.EqualTo(DSlToken.CloudProvider)
                            .Or(Token.EqualTo(DSlToken.FogProvider))
                            .Or(Token.EqualTo(DSlToken.EdgeProvider))
            from reference in TextContent
            from start in Token.EqualTo(DSlToken.LBracket)
            from nameToken in Token.EqualTo(DSlToken.ProviderName)
            from name in String
            from LocationToken in Token.EqualTo(DSlToken.ProviderLocation)
            from pl in String
            from end in Token.EqualTo(DSlToken.RBrracket)
            select new Provider { Type = type.ToStringValue(), Reference= reference, Name = name, ProviderLocation = pl};

        static PiplineParser SubPipeline { get; } =
             from begin in Token.EqualTo(DSlToken.SubPipeline)
             from name in PipelineName
             from lBracke in Token.EqualTo(DSlToken.LBracket)
             from steps in Steps
             from _ in Token.EqualTo(DSlToken.RBrracket)
             select new Pipeline { Name = name, Steps = steps };

        static PiplineParser MainPipeline { get; } =
         from begin in Token.EqualTo(DSlToken.Pipeline)
         from name in PipelineName
         from lBracke in Token.EqualTo(DSlToken.LBracket)
         from steps in Steps
         from _ in Token.EqualTo(DSlToken.RBrracket)
         select new Pipeline { Name = name, Steps = steps };


        static RootParser Dsl { get; } =
            from pipe in MainPipeline
            from subPipelines in SubPipeline.Many().OptionalOrDefault()
            from providers in Provider.Many().OptionalOrDefault()
            select new Dsl { Pipeline = pipe, SubPipelines = subPipelines, Providers= providers};


        static RootParser Pipeline { get; } = Dsl.AtEnd();



        public static bool TryParse(TokenList<DSlToken> tokens, out Dsl expr, out string error, out Position errorPosition)
        {
            var result = Pipeline(tokens);
            if (!result.HasValue)
            {
                expr = null;
                error = result.ToString();
                errorPosition = result.ErrorPosition;
                return false;
            }

            expr = result.Value;
            error = null;
            errorPosition = Position.Empty;
            return true;
        }
    }
}
