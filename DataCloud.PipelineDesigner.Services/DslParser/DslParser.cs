using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using System.Linq;

using TokenParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, object>;
using StringParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, string>;
using KeyvalueParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, System.Collections.Generic.KeyValuePair<string, string>>;
using EnvironmentParameterParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, System.Collections.Generic.Dictionary<string, string>>;
using RequirementsSubTypeParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, DataCloud.PipelineDesigner.WorkflowModel.DSL.RequirementsSubType>;
using ExecutionRequParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, DataCloud.PipelineDesigner.WorkflowModel.DSL.ExecutionRequirements>;
using ExecutionRequsParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, DataCloud.PipelineDesigner.WorkflowModel.DSL.ExecutionRequirements[]>;
using StepParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, DataCloud.PipelineDesigner.WorkflowModel.DSL.Step>;
using ImplementationParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, DataCloud.PipelineDesigner.WorkflowModel.DSL.Implementation>;
using ResourceProviderParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, DataCloud.PipelineDesigner.WorkflowModel.DSL.ResourceProvider>;
using CommunicationMediumParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, DataCloud.PipelineDesigner.WorkflowModel.DSL.CommunicationMedium>;
using PiplineParser = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, DataCloud.PipelineDesigner.WorkflowModel.DSL.Pipeline>;
using DSLModel = Superpower.TokenListParser<DataCloud.PipelineDesigner.Services.DSlToken, DataCloud.PipelineDesigner.WorkflowModel.DSL.Dsl>;
using DataCloud.PipelineDesigner.WorkflowModel.DSL;
using System.Collections.Generic;

namespace DataCloud.PipelineDesigner.Services
{
    public static class DslParser
    {
        

        public static StringParser TextContent =>
            Token.EqualTo(DSlToken.Text)
            .Select(n => n.ToStringValue());

        public static StringParser EscaptedTextContent =>
        Token.EqualTo(DSlToken.Text).Or(Token
            .EqualTo(DSlToken.Colon))
        .Select(n => n.ToStringValue());

        public static StringParser String =>
        from String in TextContent.Many()
        select string.Join(" ", String);

        public static StringParser DelimitedString =>
        from String in EscaptedTextContent.Many().Between(Token.EqualTo(DSlToken.Apostrophe), Token.EqualTo(DSlToken.Apostrophe))
        select string.Join("", String);

        public static StringParser PipelineName =>
            from name in TextContent
            select (string)name;

        public static StringParser StepType =>
            from stepType in Token.EqualTo(DSlToken.StepType)
            select stepType.ToStringValue();

        public static ImplementationParser StepImplementation =>
            from implementationToken in Token.EqualTo(DSlToken.Implementation)
            from _ in Token.EqualTo(DSlToken.Colon)
            from stepImplementation in Token.EqualTo(DSlToken.StepImplementation)
            from imageToken in Token.EqualTo(DSlToken.Image)
            from __ in Token.EqualTo(DSlToken.Colon)
            from image in DelimitedString
            select new Implementation {Type= stepImplementation.ToStringValue(), ImageName = image };


        public static StringParser StepResource =>
            from rsrc in Token.EqualTo(DSlToken.ResourceProvider)
            from _ in Token.EqualTo(DSlToken.Colon)
            from stepRsrc in TextContent.Many()
            select string.Join(" ", stepRsrc);


        public static StringParser StepPrevious =>
            from prev in Token.EqualTo(DSlToken.Previous)
            from colon in Token.EqualTo(DSlToken.Colon)
            from setpPrev in TextContent.Many()
            select string.Join(" ", setpPrev);



        public static KeyvalueParser Envs =>
            from key in String
            from _ in Token.EqualTo(DSlToken.Colon)
            from value in DelimitedString
            from comma in Token.EqualTo(DSlToken.Comma).Optional()
            select new KeyValuePair<string, string>(key, value);

        public static EnvironmentParameterParser EnvironmentParameter =>
            from envParmasToken in Token.EqualTo(DSlToken.EnvironmentParameter)
            from _ in Token.EqualTo(DSlToken.Colon)
            from start in Token.EqualTo(DSlToken.LBracket)
            from envs in Envs.Many()
            from end in Token.EqualTo(DSlToken.RBrracket)
            select envs.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);

        public static KeyvalueParser RequirementVariable =>
            from spec in Token.EqualTo(DSlToken.RequirementVariable)
            from _ in Token.EqualTo(DSlToken.Colon)
            from value in String
            select new KeyValuePair<string, string>(spec.ToStringValue(), value);

        public static RequirementsSubTypeParser RequirementSubType =>
            from subtype in Token.EqualTo(DSlToken.RequirementSubType)
            from _ in Token.EqualTo(DSlToken.Colon)
            from variable in RequirementVariable.Many()
            select new RequirementsSubType { SubType= subtype.ToStringValue(), Requirements = variable.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value) };

        public static ExecutionRequParser RequirementType =>
            from RequirementType in Token.EqualTo(DSlToken.RequirementType)
            from _ in Token.EqualTo(DSlToken.Colon)
            from req in RequirementSubType.Many()
            select new ExecutionRequirements { Type = RequirementType.ToStringValue(), SubTypeRequirements= req};

        public static ExecutionRequsParser StepExecReq =>
            from exec in Token.EqualTo(DSlToken.ExecutionRequirement)
            from _ in Token.EqualTo(DSlToken.Colon)
            from req in RequirementType.Many()
            select req;

        static StepParser Step { get; } =
            from startStepToken in Token.EqualTo(DSlToken.StartStep)
            from stepType in Token.EqualTo(DSlToken.StepType)
            from type in Token.EqualTo(DSlToken.Step) // step | subPipline
            from name in String
            from implementation in StepImplementation.OptionalOrDefault()
            from env in EnvironmentParameter.OptionalOrDefault()
            from resource in StepResource.OptionalOrDefault()
            from previous in StepPrevious.OptionalOrDefault()
            from execRequ in StepExecReq.OptionalOrDefault()
            select new Step { Name = name, Type = type.ToStringValue(),  StepType=stepType.ToStringValue(), Implementation = implementation, ResourceProvider = resource, Previous = null, ExecRequirements = execRequ , EnvParams= env };


        static ResourceProviderParser Provider { get; } =
            from resourceProvider in Token.EqualTo(DSlToken.ResourceProviderDefinition)
            from name in TextContent
            from start in Token.EqualTo(DSlToken.LBracket)
            from LocationToken in Token.EqualTo(DSlToken.ProviderLocation)
            from _ in Token.EqualTo(DSlToken.Colon)
            from pl in DelimitedString
            from mapingToken in Token.EqualTo(DSlToken.MappingLocation)
            from __ in Token.EqualTo(DSlToken.Colon)
            from mapping in DelimitedString
            from end in Token.EqualTo(DSlToken.RBrracket)
            select new ResourceProvider { Provider = resourceProvider.ToStringValue(), Name= name, MappingLocation = mapping, ProviderLocation = pl};

        static PiplineParser SubPipelineDefinition { get; } =
             from begin in Token.EqualTo(DSlToken.SubPipeline)
             from name in PipelineName
             from lBracke in Token.EqualTo(DSlToken.LBracket)
             from steps in Step.Many()
             from _ in Token.EqualTo(DSlToken.RBrracket)
             select new Pipeline { Name = name, Steps = steps };

        static CommunicationMediumParser CommunicationMedium { get; } =
            from medium in Token.EqualTo(DSlToken.Medium)
            from communicationMediumType in Token.EqualTo(DSlToken.CommunicationMediumTypes)
            select new CommunicationMedium { Type=communicationMediumType.ToStringValue()};

        static PiplineParser Pipeline { get; } =
             from pipelineToken in Token.EqualTo(DSlToken.Pipeline)
             from name in PipelineName
             from lBracke in Token.EqualTo(DSlToken.LBracket)
             from communicationMediumToken in Token.EqualTo(DSlToken.CommunicationMedium)
             from colonC in Token.EqualTo(DSlToken.Colon)
             from communicationMedium in CommunicationMedium
             from env in EnvironmentParameter.OptionalOrDefault()
             from setpsToken in Token.EqualTo(DSlToken.Steps)
             from colonS in Token.EqualTo(DSlToken.Colon)
             from steps in Step.Many()
             from _ in Token.EqualTo(DSlToken.RBrracket)
             select new Pipeline { Name = name, Steps = steps, CommunicationMedium= communicationMedium };


        static DSLModel Dsl { get; } =
            from pipeline in Pipeline
            from subPipelines in SubPipelineDefinition.Many().OptionalOrDefault()
            from providers in Provider.Many().OptionalOrDefault()
            select new Dsl { Pipeline = pipeline, SubPipelines = subPipelines, ResourceProvider = providers};


        static DSLModel DSLModel { get; } = Dsl.AtEnd();



        public static bool TryParse(TokenList<DSlToken> tokens, out Dsl expr, out string error, out Position errorPosition)
        {

            
            var result = DSLModel(tokens);
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
