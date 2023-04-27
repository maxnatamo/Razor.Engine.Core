using RazorEngineCore.Tests.Models;

namespace RazorEngineCore.Tests
{
    public class TestTemplateModelNamespace
    {
        [Fact]
        public void Compile_Returns_GivenNestedTypeModel()
        {
            // Arrange
            string content = "Hello @Model.Name";
            var template = new RazorEngine().Compile<RazorEngineTemplateBase<NestedTestModel.TestModelInnerClass1.TestModelInnerClass2>>(content);

            // Act
            string result = template.Run(instance =>
            {
                instance.Model = new NestedTestModel.TestModelInnerClass1.TestModelInnerClass2()
                {
                    Name = "Hello",
                };
            });

            // Assert
            result.Should().Be("Hello Hello");
        }

        [Fact]
        public void Compile_Returns_GivenModelWithoutNamespace()
        {
            // Arrange
            string content = "Hello @Model.Name";
            var template = new RazorEngine().Compile<RazorEngineTemplateBase<TestModelWithoutNamespace>>(content);

            // Act
            string result = template.Run(instance =>
            {
                instance.Model = new TestModelWithoutNamespace()
                {
                    Name = "Hello",
                };
            });

            // Assert
            result.Should().Be("Hello Hello");
        }
    }
}