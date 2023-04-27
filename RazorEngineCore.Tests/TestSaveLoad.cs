namespace RazorEngineCore.Tests
{
    public class TestSaveLoad
    {
        [Fact]
        public void SaveToStream_ReturnsSameTemplate_GivenTemplate()
        {
            // Arrange
            var initialTemplate = new RazorEngine().Compile("Hello @Model.Name");

            MemoryStream memoryStream = new MemoryStream();
            initialTemplate.SaveToStream(memoryStream);
            memoryStream.Position = 0;

            var loadedTemplate = RazorEngineCompiledTemplate.LoadFromStream(memoryStream);

            // Act
            string initialTemplateResult = initialTemplate.Run(new { Name = "Alex" });
            string loadedTemplateResult = loadedTemplate.Run(new { Name = "Alex" });

            // Assert
            initialTemplateResult.Should().BeEquivalentTo(loadedTemplateResult);
        }

        [Fact]
        public async Task SaveToStreamAsync_ReturnsSameTemplate_GivenTemplate()
        {
            // Arrange
            var initialTemplate = await new RazorEngine().CompileAsync("Hello @Model.Name");

            MemoryStream memoryStream = new MemoryStream();
            await initialTemplate.SaveToStreamAsync(memoryStream);
            memoryStream.Position = 0;

            var loadedTemplate = await RazorEngineCompiledTemplate.LoadFromStreamAsync(memoryStream);

            // Act
            string initialTemplateResult = await initialTemplate.RunAsync(new { Name = "Alex" });
            string loadedTemplateResult = await loadedTemplate.RunAsync(new { Name = "Alex" });

            // Assert
            initialTemplateResult.Should().BeEquivalentTo(loadedTemplateResult);
        }

        [Fact]
        public void SaveToFile_ReturnsSameTemplate_GivenTemplate()
        {
            // Arrange
            var initialTemplate = new RazorEngine().Compile("Hello @Model.Name");
            initialTemplate.SaveToFile("testTemplate.dll");
            var loadedTemplate = RazorEngineCompiledTemplate.LoadFromFile("testTemplate.dll");

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
            var initialTemplate = await new RazorEngine().CompileAsync("Hello @Model.Name");
            await initialTemplate.SaveToFileAsync("testTemplate.dll");
            var loadedTemplate = await RazorEngineCompiledTemplate.LoadFromFileAsync("testTemplate.dll");

            // Act
            string initialTemplateResult = await initialTemplate.RunAsync(new { Name = "Alex" });
            string loadedTemplateResult = await loadedTemplate.RunAsync(new { Name = "Alex" });

            // Assert
            initialTemplateResult.Should().BeEquivalentTo(loadedTemplateResult);
        }
    }
}