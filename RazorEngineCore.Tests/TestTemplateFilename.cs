namespace RazorEngineCore.Tests
{
    public class TestTemplateFilename
    {
        [Fact]
        public void TestSettingTemplateFilename()
        {
            // Arrange
            RazorEngine razorEngine = new RazorEngine();

            // Act
            Action act = () => razorEngine.Compile(
                "@{ this is a syntaxerror }", 
                builder =>
                {
                    builder.Options.TemplateFilename = "templatefilenameset.txt";
                });

            // Assert
            act.Should().Throw<Exception>();
        }
    }
}
