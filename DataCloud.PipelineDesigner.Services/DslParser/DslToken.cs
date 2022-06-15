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

        [Token(Example = ",")]
        Comma,

        [Token(Example = "Pipeline")]
        Pipeline,

        [Token(Example = "communicationMedium")]
        CommunicationMedium,

        [Token(Example = "medium")]
        Medium,

        [Token(Example = " MESSAGE_QUEUE | DISTRIBUTED_FILE_SYSTEM | WEB_SERVICE ")]
        CommunicationMediumTypes,

        [Token(Example = "SubPipeline")]
        SubPipeline,

        [Token(Example = "providerLocation:")]
        ProviderLocation,

        [Token(Example = "mappingLocation:")]
        MappingLocation,

        [Token(Example = "{")]
        LBracket,
                
        [Token(Example = "}")]
        RBrracket,
        
        [Token(Example = ":")]
        Colon,

        [Token(Example = "steps")]
        Steps,

        [Token(Example = "-")]
        StartStep,

        [Token(Example = "step | subPipeline")]
        Step,

        [Token(Example = " data-source | data-processing | data-sink ")]
        StepType,

        [Token(Example = "image:")]
        Image,
                
        [Token(Example = "implementation")]
        Implementation,
                
        [Token(Example = "ContainerImplementation | ")]
        StepImplementation,
                
        [Token(Example = "environmentParameters")]
        EnvironmentParameter,
           
        [Token(Example = "resourceProvider")]
        ResourceProvider,

        [Token(Example = "CloudProvider, EdgeProvider, FogProvider")]
        ResourceProviderDefinition,

        [Token(Example = "previous")]
        Previous,

        [Token(Example = "executionRequirement")]
        ExecutionRequirement,

        [Token(Example = "hardRequirements | softRequirements")]
        RequirementType,

        [Token(Example = "horizontalScale | verticalScale | qualitativeRequirement | quantitativeRequirement")]
        RequirementSubType,

        [Token(Example = "quantitativeRequirement:")]
        QuantitativeRequirement,

        [Token(Example = "min-instance | max-instance | ...")]
        RequirementVariable,

        [Token(Example = "\r")]
        Return,




        [Token(Example = "foo:, bar: -> all the other key with colon")]
        Key,





        Text

    }
}