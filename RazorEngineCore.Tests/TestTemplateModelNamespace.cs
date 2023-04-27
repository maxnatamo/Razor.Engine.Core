using RazorEngineCore.Tests.Models;

namespace RazorEngineCore.Tests
{
    public class TestTemplateModelNamespace
    {
        [Fact]
        public void TestModelNestedTypes()
        {
            IRazorEngine razorEngine = new RazorEngine();
            string content = "Hello @Model.Name";

            IRazorEngineCompiledTemplate<RazorEngineTemplateBase<NestedTestModel.TestModelInnerClass1.TestModelInnerClass2>> template2 = razorEngine.Compile<RazorEngineTemplateBase<NestedTestModel.TestModelInnerClass1.TestModelInnerClass2>>(content);

            string result = template2.Run(instance =>
            {
                instance.Model = new NestedTestModel.TestModelInnerClass1.TestModelInnerClass2()
                {
                    Name = "Hello",
                };
            });

            result.Should().Be("Hello Hello");
        }

        [Fact]
        public void TestModelNoNamespace()
        {
            IRazorEngine razorEngine = new RazorEngine();
            string content = "Hello @Model.Name";

            IRazorEngineCompiledTemplate<RazorEngineTemplateBase<TestModelWithoutNamespace>> template2 = razorEngine.Compile<RazorEngineTemplateBase<TestModelWithoutNamespace>>(content);

            string result = template2.Run(instance =>
            {
                instance.Model = new TestModelWithoutNamespace()
                {
                    Name = "Hello",
                };
            });

            result.Should().Be("Hello Hello");
        }
    }
}