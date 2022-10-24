using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace DataCloud.PipelineDesigner.Services
{
    public static class DslTokenizer
    {

        static Tokenizer<DSlToken> Tokenizer { get; } = new TokenizerBuilder<DSlToken>()
            .Match(Character.EqualTo('{'), DSlToken.LBracket)
            .Match(Character.EqualTo('}'), DSlToken.RBrracket)
            .Match(Character.EqualTo('*'), DSlToken.Asterisk)
            .Match(Character.EqualTo('/'), DSlToken.Slash)
            .Match(Character.EqualTo('\''), DSlToken.Apostrophe)
            .Match(Span.EqualTo("Pipeline"), DSlToken.Pipeline)
            .Match(Span.EqualTo("SubPipeline"), DSlToken.SubPipeline)

            .Match(Span.EqualTo("communicationMedium"), DSlToken.CommunicationMedium)
            .Match(Span.EqualTo("medium"), DSlToken.Medium)
            .Match(Span.EqualTo("MESSAGE_QUEUE"), DSlToken.CommunicationMediumTypes)
            .Match(Span.EqualTo("DISTRIBUTED_FILE_SYSTEM"), DSlToken.CommunicationMediumTypes)
            .Match(Span.EqualTo("WEB_SERVICE"), DSlToken.CommunicationMediumTypes)

            .Match(Span.EqualTo("environmentParameters"), DSlToken.EnvironmentParameter)

            .Match(Span.EqualTo("steps"), DSlToken.Steps)
            .Match(Character.EqualTo('-'), DSlToken.StartStep)

            .Match(Span.EqualTo("step"), DSlToken.Step)
            .Match(Span.EqualTo("subPipeline"), DSlToken.StartStep)

            .Match(Span.EqualTo("data-source"), DSlToken.StepType)
            .Match(Span.EqualTo("data-processing"), DSlToken.StepType)
            .Match(Span.EqualTo("data-sink"), DSlToken.StepType)

            .Match(Span.EqualTo("container-implementation"), DSlToken.StepImplementation)
            .Match(Span.EqualTo("implementation"), DSlToken.Implementation)


            .Match(Span.EqualTo("image"), DSlToken.Image)

            .Match(Span.EqualTo("resourceProvider"), DSlToken.ResourceProvider)
            .Match(Span.EqualTo("CloudProvider"), DSlToken.ResourceProviderDefinition)
            .Match(Span.EqualTo("EdgeProvider"), DSlToken.ResourceProviderDefinition)
            .Match(Span.EqualTo("FogProvider"), DSlToken.ResourceProviderDefinition)
            .Match(Span.EqualTo("mappingLocation"), DSlToken.MappingLocation)
            .Match(Span.EqualTo("providerLocation"), DSlToken.ProviderLocation)

            .Match(Span.EqualTo("previous"), DSlToken.Previous)

            .Match(Span.EqualTo("executionRequirement"), DSlToken.ExecutionRequirement)
            .Match(Span.EqualTo("hardRequirements"), DSlToken.RequirementType)
            .Match(Span.EqualTo("softRequirements"), DSlToken.RequirementType)
            .Match(Span.EqualTo("quantitativeRequirement"), DSlToken.RequirementSubType)
            .Match(Span.EqualTo("qualitativeRequirement"), DSlToken.RequirementSubType)
            .Match(Span.EqualTo("horizontalScale"), DSlToken.RequirementSubType)
            .Match(Span.EqualTo("imageRequirement"), DSlToken.RequirementSubType)
            .Match(Span.EqualTo("osRequirement"), DSlToken.RequirementSubType)
            .Match(Span.EqualTo("networkRequirement"), DSlToken.RequirementSubType)
            .Match(Span.EqualTo("verticalScale"), DSlToken.RequirementSubType)
            .Match(Span.EqualTo("max-instance"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("min-instance"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("min-cpu"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("max-cpu"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("min-ram-mb"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("max-ram-mb"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("min-cores"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("max-cores"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("min-storage-mb"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("max-storage-mb"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("cpu-frequency"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("cpu-no-core"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("gpu-availability"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("min-bandwidth"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("max-bandwidth"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("min-latency"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("max-latency"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("min-benchmark"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("max-benchmark"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("cpu-architecture"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("gpu-architecture"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("os-type"), DSlToken.RequirementVariable)
            .Match(Span.EqualTo("is-64"), DSlToken.RequirementVariable)
            .Match(Character.EqualTo(','), DSlToken.Comma)
            .Match(Character.EqualTo(':'), DSlToken.Colon)
            .Match(Span.WithoutAny(StringSeparator), DSlToken.Text)
            .Ignore(Span.WhiteSpace)
            .Ignore(Character.EqualTo('.'))
            .Ignore(Character.EqualTo('?'))
            .Ignore(Character.EqualTo('\n'))
            .Build();



        public static bool StringSeparator(char c)
        {
            if (char.IsWhiteSpace(c) || c.Equals(':') || c.Equals(',') || c.Equals('\''))
                return true;
            else
                return false;
        }


        public static Result<TokenList<DSlToken>> TryTokenize(string source)
        {
            return Tokenizer.TryTokenize(source);
        }
    }
}