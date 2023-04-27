using RazorEngineCore.Tests.Models;

namespace RazorEngineCore.Tests
{
    public class TestTemplateNamespace
    {
        [Fact]
        public void TestSettingTemplateNamespace()
        {
            RazorEngine razorEngine = new RazorEngine();

            IRazorEngineCompiledTemplate initialTemplate = razorEngine.Compile("@{ var message = \"OK\"; }@message",
                builder => { builder.Options.TemplateNamespace = "Test.Namespace"; });

            var result = initialTemplate.Run();

            result.Should().Be("OK");
        }

        [Fact]
        public void TestSettingTemplateNamespaceT()
        {
            RazorEngine razorEngine = new RazorEngine();

            var initialTemplate = razorEngine.Compile<TestTemplate2>("@{ var message = \"OK\"; }@message",
                builder => { builder.Options.TemplateNamespace = "Test.Namespace"; });

            var result = initialTemplate.Run(a => { });

            result.Should().Be("OK");
        }
    }
}
