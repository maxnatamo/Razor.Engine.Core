namespace RazorEngineCore.Tests
{
    public class TestSaveLoad
    {
        [Fact]
        public void TestSaveToStream()
        {
            // Arrange
            RazorEngine razorEngine = new RazorEngine();
            IRazorEngineCompiledTemplate initialTemplate = razorEngine.Compile("Hello @Model.Name");
            
            MemoryStream memoryStream = new MemoryStream();
            initialTemplate.SaveToStream(memoryStream);
            memoryStream.Position = 0;

            IRazorEngineCompiledTemplate loadedTemplate = RazorEngineCompiledTemplate.LoadFromStream(memoryStream);

            // Act
            string initialTemplateResult = initialTemplate.Run(new { Name = "Alex" });
            string loadedTemplateResult = loadedTemplate.Run(new { Name = "Alex" });

            // Assert
            initialTemplateResult.Should().BeEquivalentTo(loadedTemplateResult);
        }

        [Fact]
        public async Task TestSaveToStreamAsync()
        {
            RazorEngine razorEngine = new RazorEngine();
            IRazorEngineCompiledTemplate initialTemplate = await razorEngine.CompileAsync("Hello @Model.Name");

            MemoryStream memoryStream = new MemoryStream();
            await initialTemplate.SaveToStreamAsync(memoryStream);
            memoryStream.Position = 0;

            IRazorEngineCompiledTemplate loadedTemplate = await RazorEngineCompiledTemplate.LoadFromStreamAsync(memoryStream);

            string initialTemplateResult = await initialTemplate.RunAsync(new { Name = "Alex" });
            string loadedTemplateResult = await loadedTemplate.RunAsync(new { Name = "Alex" });

            initialTemplateResult.Should().BeEquivalentTo(loadedTemplateResult);
        }

        [Fact]
        public void TestSaveToFile()
        {
            RazorEngine razorEngine = new RazorEngine();
            IRazorEngineCompiledTemplate initialTemplate = razorEngine.Compile("Hello @Model.Name");

            initialTemplate.SaveToFile("testTemplate.dll");

            IRazorEngineCompiledTemplate loadedTemplate = RazorEngineCompiledTemplate.LoadFromFile("testTemplate.dll");

            string initialTemplateResult = initialTemplate.Run(new { Name = "Alex" });
            string loadedTemplateResult = loadedTemplate.Run(new { Name = "Alex" });

            initialTemplateResult.Should().BeEquivalentTo(loadedTemplateResult);
        }

        [Fact]
        public async Task TestSaveToFileAsync()
        {
            RazorEngine razorEngine = new RazorEngine();
            IRazorEngineCompiledTemplate initialTemplate = await razorEngine.CompileAsync("Hello @Model.Name");
            
            await initialTemplate.SaveToFileAsync("testTemplate.dll");

            IRazorEngineCompiledTemplate loadedTemplate = await RazorEngineCompiledTemplate.LoadFromFileAsync("testTemplate.dll");

            string initialTemplateResult = await initialTemplate.RunAsync(new { Name = "Alex" });
            string loadedTemplateResult = await loadedTemplate.RunAsync(new { Name = "Alex" });

            initialTemplateResult.Should().BeEquivalentTo(loadedTemplateResult);
        }
    }
}