using RazorEngineCore.Tests.Models;

namespace RazorEngineCore.Tests
{
    public class TestTemplateNamespace
    {
        [Fact]
        public void Compile_ReturnsValue_GivenVariableExpansion()
        {
            // Arrange
            var initialTemplate = new RazorEngine().Compile(
                "@{ var message = \"OK\"; }@message",
                options =>
                {
                    options.TemplateNamespace = "Test.Namespace";
                });

            // Act
            var result = initialTemplate.Run();

            // Assert
            result.Should().Be("OK");
        }

        [Fact]
        public void Compile_ReturnsValue_GivenVariableExpansionWithType()
        {
            // Arrange
            var initialTemplate = new RazorEngine().Compile<TestTemplate2>(
                "@{ var message = \"OK\"; }@message",
                options =>
                {
                    options.TemplateNamespace = "Test.Namespace";
                });

            // Act
            var result = initialTemplate.Run(a => { });

            // Assert
            result.Should().Be("OK");
        }
    }
}