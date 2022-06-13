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
            .Match(Span.EqualTo("-\t"), DSlToken.StepSign)
            .Match(Character.EqualTo('*'), DSlToken.Asterisk)
            .Match(Character.EqualTo('/'), DSlToken.Slash)
            .Match(Character.EqualTo('='), DSlToken.Equal)
            .Match(Span.EqualTo("Pipeline"), DSlToken.Pipeline)
            .Match(Span.EqualTo("SubPipeline"), DSlToken.SubPipeline)
            .Match(Span.EqualTo("CloudProvider"), DSlToken.CloudProvider)
            .Match(Span.EqualTo("EdgeProvider"), DSlToken.EdgeProvider)
            .Match(Span.EqualTo("FogProvider"), DSlToken.FogProvider)
            .Match(Span.EqualTo("provider-location: "), DSlToken.ProviderLocation)
            .Match(Span.EqualTo("name: "), DSlToken.ProviderName)
            .Match(Span.EqualTo("steps:"), DSlToken.Steps)
            .Match(Span.EqualTo("image: "), DSlToken.Image)
            .Match(Span.EqualTo("implementation: "), DSlToken.Implem)
            .Match(Span.EqualTo("environmentParameters: "), DSlToken.EnvParam)
            .Match(Span.EqualTo("executionRequirements:"), DSlToken.ExecRequ)
            .Match(Span.EqualTo("resourceProvider: "), DSlToken.Resrc)
            .Match(Span.EqualTo("previous: "), DSlToken.Prev)
            .Match(Character.EqualTo(':'), DSlToken.Colon)
            .Match(Span.NonWhiteSpace, DSlToken.Text)
            .Ignore(Span.WhiteSpace)
            .Ignore(Character.EqualTo(','))
            .Ignore(Character.EqualTo('.'))
            .Ignore(Character.EqualTo('?'))
            .Ignore(Character.EqualTo('\n'))
            .Build();

        public static Result<TokenList<DSlToken>> TryTokenize(string source)
        {
            return Tokenizer.TryTokenize(source);
        }
    }
}