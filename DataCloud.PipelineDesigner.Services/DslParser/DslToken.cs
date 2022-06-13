using Superpower.Display;
using Superpower.Parsers;

namespace DataCloud.PipelineDesigner.Services
{
    public enum DSlToken
    {

        [Token(Example = "*")]
        Asterisk,

        [Token(Example = "/")]
        Slash,

        [Token(Example = "(")]
        LParen,

        [Token(Example = ")")]
        RParen,

        [Token(Example = "Pipeline")]
        Pipeline,

        [Token(Example = "SubPipeline")]
        SubPipeline,

        [Token(Example = "CloudProvider")]
        CloudProvider,

        [Token(Example = "EdgeProvider")]
        EdgeProvider,

        [Token(Example = "FogProvider")]
        FogProvider,

        [Token(Example = "provider-location: ")]
        ProviderLocation,

        [Token(Example = "name: ")]
        ProviderName,

        [Token(Example = "{")]
        LBracket,
                
        [Token(Example = "}")]
        RBrracket,
                        
        Equal,
        [Token(Example = "steps:")]
        Steps,
                
        [Token(Example = "-\t")]
        StepSign,
        
        [Token(Example = ":")]
        Colon,
                
        [Token(Example = "image: ")]
        Image,
                
        [Token(Example = "implementation: ")]
        Implem,
                
        [Token(Example = "environmentParameters: ")]
        EnvParam,
           
        [Token(Example = "executionRequirements: ")]
        ExecRequ,
           
        [Token(Example = "resourceProvider: ")]
        Resrc,
           
        [Token(Example = "previous: ")]
        Prev,

        [Token(Example = "\r")]
        Return,

        Text

    }
}