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

        public static StringParser PipelineName =>
            from name in TextContent
            select (string)name;

        public static StringParser StepName =>
            from stepName in TextContent.Many()
            from _ in Token.EqualTo(DSlToken.Return)
            select string.Join(" ", stepName);

        public static StringParser StepImplementation =>
            from impl in Token.EqualTo(DSlToken.Implem)
            from stepImpl in TextContent.Many()
            from _ in Token.EqualTo(DSlToken.Return)
            select string.Join(" ", stepImpl);

        public static StringParser StepImage =>
            from image in Token.EqualTo(DSlToken.Image)
            from stepImage in TextContent.Many()
            from _ in Token.EqualTo(DSlToken.Return)
            select string.Join(" ", stepImage);

        public static StringParser StepResource =>
            from rsrc in Token.EqualTo(DSlToken.Resrc)
            from stepRsrc in TextContent.Many()
            from _ in Token.EqualTo(DSlToken.Return)
            select string.Join(" ", stepRsrc);


        public static StringParser StepPrevious =>
            from prev in Token.EqualTo(DSlToken.Prev)
            from setpPrev in TextContent.Many()
            from _ in Token.EqualTo(DSlToken.Return)
            select string.Join(" ", setpPrev);



        public static KeyvalueParser Envs =>
            from value in Token.EqualTo(DSlToken.Text)
            from _ in Token.EqualTo(DSlToken.Return)
            select new KeyValuePair<string, string>(value.ToStringValue().Split("=")[0], value.ToStringValue().Split("=")[1]);

        public static EnvParamsParser StepEnvParam =>
            from parm in Token.EqualTo(DSlToken.EnvParam)
            from start in Token.EqualTo(DSlToken.LBracket)
            from ret in Token.EqualTo(DSlToken.Return)
            from env in Envs.Many()
            from end in Token.EqualTo(DSlToken.RBrracket)
            from _ in Token.EqualTo(DSlToken.Return)
            select env.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);

        public static KeyvalueParser ExecSpec =>
            from value in TextContent.Many()
            from _ in Token.EqualTo(DSlToken.Return)
            select new KeyValuePair<string, string>(value[0], value[1]);

        public static ExecutionRequParser Req =>
            from type in TextContent
            from subtype in TextContent
            from start in Token.EqualTo(DSlToken.LBracket)
            from retur in Token.EqualTo(DSlToken.Return)
            from spec in ExecSpec.Many()
            from end in Token.EqualTo(DSlToken.RBrracket)
            from _ in Token.EqualTo(DSlToken.Return)
            select new ExecutionRequirements { Type = type, SubType = subtype, Requirements = spec.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value)};

        public static ExecutionRequsParser StepExecReq =>
            from exec in Token.EqualTo(DSlToken.ExecRequ)
            from ret in Token.EqualTo(DSlToken.Return)
            from req in Req.Many()
            select req;

        static StepParser Step { get; } =
            from _ in Token.EqualTo(DSlToken.Return)
            .Or(Token.EqualTo(DSlToken.StepSign))
            .Optional()
            from name in StepName
            from implementation in StepImplementation
            from image in StepImage
            from env in StepEnvParam
            from execRequ in StepExecReq.OptionalOrDefault()
            from resource in StepResource
            from previous in StepPrevious.OptionalOrDefault()
            select new Step { Name = name, Implementation = implementation, Image = image, ResourceProvider = resource, Previous = previous, ExecRequirements = execRequ , EnvParams= env };

        static StepsParser Steps { get; } =
            from stepSign in Token.EqualTo(DSlToken.Steps)
            from ret in Token.EqualTo(DSlToken.Return)
            from steps in Step.Many()
            select steps;

        static RootParser Dsl { get; } =
            from begin in Token.EqualTo(DSlToken.Pipeline)
            from name in PipelineName
            from lBracke in Token.EqualTo(DSlToken.LBracket)
            from ret in Token.EqualTo(DSlToken.Return)
            from steps in Steps
            from _ in Token.EqualTo(DSlToken.RBrracket)
            from end in Token.EqualTo(DSlToken.Return).OptionalOrDefault()
            select new Dsl { Name = name, Steps = steps};


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
