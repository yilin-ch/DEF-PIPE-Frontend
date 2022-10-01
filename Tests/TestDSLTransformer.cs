using DataCloud.PipelineDesigner.Services;
using DataCloud.PipelineDesigner.WorkflowModel.DSL;
using Xunit;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

namespace DataCloud.PipelineDesigner.Tests
{
    public class TestDSLTransformer
    {

        [Fact]
        public void TestSimpleDSL()
        {
            var dsl = new Dsl();
            dsl.Pipeline.Name = "TestSimpleDSL";
            dsl.Pipeline.CommunicationMedium = new CommunicationMedium { Type = "WEB_SERVICE" };
            dsl.Pipeline.EnvParams = new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } };

            var transfomer = new DSLTransfomer(dsl);

            var result = transfomer.Transform();


            Assert.Equal("Pipeline TestSimpleDSL {" +
                        "\r\n\tcommunicationMedium: medium WEB_SERVICE" +
                         "\r\n\tenvironmentParameters: {" +
                         "\r\n\t\tkey1: 'value1'" +
                         "\r\n\t\tkey2: 'value2'" +
                         "\r\n\t}" +
                        "\r\n\tsteps:" +
                        "\r\n}" +
                        "\r\n", result);

        }

        [Fact]
        public void TestDSLStep()
        {
            var dsl = new Dsl();
            dsl.Pipeline.Name = "TestDSLStep";
            dsl.Pipeline.CommunicationMedium = new CommunicationMedium { Type = "WEB_SERVICE" };
            dsl.Pipeline.Steps = new[] {
                new Step { Name = "step1",
                           StepType = "data-source",
                           Implementation = new Implementation { Type = "container-implementation", ImageName = "myimage:latest" }}
            };
            var transfomer = new DSLTransfomer(dsl);

            var result = transfomer.Transform();


            Assert.Equal("Pipeline TestDSLStep {" +
                        "\r\n\tcommunicationMedium: medium WEB_SERVICE" +
                        "\r\n\tsteps:" +
                        "\r\n\t\t- data-source step step1" +
                        "\r\n\t\timplementation: container-implementation image: 'myimage:latest'" +
                        "\r\n}" +
                        "\r\n", result);

        }
    }
}