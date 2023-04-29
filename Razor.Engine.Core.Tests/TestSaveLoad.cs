namespace RazorEngineCore.Tests
{
    public class TestSaveLoad
    {
        [Fact]
        public void SaveToStream_ReturnsSameTemplate_GivenTemplate()
        {
            // Arrange
            var initialTemplate = new RazorEngine().Compile("Hello @Model.Name");

            using MemoryStream memoryStream = new MemoryStream();
            initialTemplate.SaveToStream(memoryStream);
            memoryStream.Position = 0;

            var loadedTemplate = RazorEngineCompiledTemplate.LoadFromStream(memoryStream, "TemplateNamespace.Template");

            // Act
            string initialTemplateResult = initialTemplate.Run(new { Name = "Alex" });
            string loadedTemplateResult = loadedTemplate.Run(new { Name = "Alex" });

            // Assert
            initialTemplateResult.Should().BeEquivalentTo(loadedTemplateResult);
        }

        [Fact]
        public void SaveToFile_ReturnsSameTemplate_GivenTemplate()
        {
            // Arrange
            string assemblyFile = Path.GetRandomFileName();

            var initialTemplate = new RazorEngine().Compile("Hello @Model.Name");
            initialTemplate.SaveToFile(assemblyFile);
            var loadedTemplate = RazorEngineCompiledTemplate.LoadFromFile(assemblyFile, "TemplateNamespace.Template");

            // Act
            string initialTemplateResult = initialTemplate.Run(new { Name = "Alex" });
            string loadedTemplateResult = loadedTemplate.Run(new { Name = "Alex" });

            // Assert
            initialTemplateResult.Should().BeEquivalentTo(loadedTemplateResult);
        }

        [Fact]
        public async Task SaveToFileAsync_ReturnsSameTemplate_GivenTemplate()
        {
            // Arrange
            string assemblyFile = Path.GetRandomFileName();

            var initialTemplate = await new RazorEngine().CompileAsync("Hello @Model.Name");
            await initialTemplate.SaveToFileAsync(assemblyFile);
            var loadedTemplate = await RazorEngineCompiledTemplate.LoadFromFileAsync(assemblyFile, "TemplateNamespace.Template");

            // Act
            string initialTemplateResult = await initialTemplate.RunAsync(new { Name = "Alex" });
            string loadedTemplateResult = await loadedTemplate.RunAsync(new { Name = "Alex" });

            // Assert
            initialTemplateResult.Should().BeEquivalentTo(loadedTemplateResult);
        }
    }
}