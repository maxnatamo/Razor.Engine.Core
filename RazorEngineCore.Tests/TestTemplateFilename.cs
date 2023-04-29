namespace RazorEngineCore.Tests
{
    public class TestTemplateFilename
    {
        [Fact]
        public void Compile_ThrowsException_GivenSyntaxError()
        {
            // Arrange
            RazorEngine razorEngine = new RazorEngine();

            // Act
            Action act = () => razorEngine.Compile(
                "@{ this is a syntaxerror }",
                options =>
                {
                    options.TemplateFilename = "templatefilenameset.txt";
                });

            // Assert
            act.Should().Throw<Exception>();
        }
    }
}