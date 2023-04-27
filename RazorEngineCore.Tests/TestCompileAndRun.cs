using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

using RazorEngineCore.Tests.Models;

namespace RazorEngineCore.Tests
{
    public class TestCompileAndRun
    {
        [Fact]
        public void Compile_DoesntThrow()
        {
            // Arrange
            RazorEngine razorEngine = new RazorEngine();

            // Act
            Action act = () => razorEngine.Compile("Hello @Model.Name");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public async Task CompileAsync_DoesntThrow()
        {
            // Arrange
            RazorEngine razorEngine = new RazorEngine();

            // Act
            Func<Task> act = async () => await razorEngine.CompileAsync("Hello @Model.Name");

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public void Compile_Returns_GivenHtmlLiteral()
        {
            // Arrange
            IRazorEngineCompiledTemplate template = new RazorEngine().Compile("<h1>Hello @Model.Name</h1>");

            // Act
            string actual = template.Run(new { Name = "Alex" });

            // Assert
            actual.Should().Be("<h1>Hello Alex</h1>");
        }

        [Fact]
        public void Compile_Returns_GivenInAttributeVariables()
        {
            // Arrange
            string content = "<div class=\"circle\" style=\"background-color: hsla(@Model.Colour, 70%,   80%,1);\">";
            IRazorEngineCompiledTemplate template = new RazorEngine().Compile(content);

            // Act
            string actual = template.Run(new { Colour = 88 });

            // Assert
            actual.Should().Be("<div class=\"circle\" style=\"background-color: hsla(88, 70%,   80%,1);\">");
        }

        [Fact]
        public void Compile_Returns_GivenInAttributeVariables2()
        {
            // Arrange
            IRazorEngineCompiledTemplate template = new RazorEngine().Compile("<img src='@(\"test\")'>");

            // Act
            string actual = template.Run(new { Colour = 88 });

            // Assert
            actual.Should().Be("<img src='test'>");
        }

        [Fact]
        public void Compile_Returns_GivenHtmlAttribute()
        {
            // Arrange
            IRazorEngineCompiledTemplate template = new RazorEngine().Compile("<div title=\"@Model.Name\">Hello</div>");

            // Act
            string actual = template.Run(new { Name = "Alex" });

            // Assert
            actual.Should().Be("<div title=\"Alex\">Hello</div>");
        }

        [Fact]
        public void Compile_Returns_GivenPlainDynamicModel()
        {
            // Assert
            IRazorEngineCompiledTemplate template = new RazorEngine().Compile("Hello @Model.Name");

            // Act
            string actual = template.Run(new { Name = "Alex" });

            // Assert
            actual.Should().Be("Hello Alex");
        }

        [Fact]
        public void Compile_Returns_GivenStructList()
        {
            // Arrange
            var model = new
            {
                Items = new[]
                {
                    new Item { Name = "Bob" },
                    new Item { Name = "Alice" }
                }
            };
            var template = new RazorEngine().Compile("@foreach(var item in Model.Items) { @item.Name }");

            // Act
            var result = template.Run(model);

            // Assert
            result.Should().Be("BobAlice");
        }

        [Fact]
        public void Compile_Returns_GivenNestedDynamicModel()
        {
            // Arrange
            var model = new
            {
                Name = "Alex",
                Membership = new
                {
                    Level = "Gold"
                }
            };

            var template = new RazorEngine().Compile("Name: @Model.Name, Membership: @Model.Membership.Level");

            // Act
            string actual = template.Run(model);

            // Assert
            actual.Should().Be("Name: Alex, Membership: Gold");
        }

        [Fact]
        public void Compile_Returns_GivenNullModel()
        {
            // Arrange
            var template = new RazorEngine().Compile("Name: @Model");

            // Act
            string actual = template.Run(null);

            // Assert
            actual.Should().Be("Name: ");
        }

        [Fact]
        public void Compile_Returns_GivenNullablePropertyWithValue()
        {
            // Arrange
            DateTime? dateTime = DateTime.Now;
            var template = new RazorEngine().Compile<TestTemplate2>("DateTime: @Model.DateTime.Value.ToString()");

            // Act
            string actual = template.Run(instance => instance.Model = new TestModel() { DateTime = dateTime });

            // Assert
            actual.Should().Be($"DateTime: {dateTime.ToString()}");
        }

        [Fact]
        public void Compile_Returns_GivenNullablePropertyWithoutValue()
        {
            // Arrange
            DateTime? dateTime = null;
            var template = new RazorEngine().Compile<TestTemplate2>("DateTime: @Model.DateTime");

            // Act
            string actual = template.Run(instance => instance.Model = new TestModel() { DateTime = dateTime });

            // Assert
            actual.Should().Be($"DateTime: {dateTime.ToString()}");
        }

        [Fact]
        public void Compile_Returns_GivenNullNestedModel()
        {
            // Arrange
            var template = new RazorEngine().Compile("Name: @Model.user");

            // Act
            string actual = template.Run(new { user = (object?) null });

            // Assert
            actual.Should().Be("Name: ");
        }

        [Fact]
        public void Compile_Returns_GivenListModel()
        {
            // Arrange
            var model = new
            {
                Items = new[]
                {
                    new
                    {
                        Key = "K1"
                    },
                    new
                    {
                        Key = "K2"
                    }
                }
            };

            string expected = @"
    <div>K1</div>
    <div>K2</div>
";

            var template = new RazorEngine().Compile(@"
@foreach (var item in Model.Items)
{
    <div>@item.Key</div>
}");

            // Act
            string actual = template.Run(model);

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void Compile_Returns_GivenDictionaryModel()
        {
            // Arrange
            var model = new
            {
                Dictionary = new Dictionary<string, object>()
                {
                    { "K1", "V1"},
                    { "K2", "V2"},
                }
            };

            string expected = @"
<div>K1</div>
<div>K2</div>
<div>V1</div>
<div>V2</div>
";

            var template = new RazorEngine().Compile(@"
@foreach (var key in Model.Dictionary.Keys)
{
<div>@key</div>
}
<div>@Model.Dictionary[""K1""]</div>
<div>@Model.Dictionary[""K2""]</div>
");

            // Act
            string actual = template.Run(model);

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void Compile_Returns_GivenDictionaryModelWithObjectValues()
        {
            // Arrange
            var model = new
            {
                Dictionary = new Dictionary<string, object>()
                {
                    { "K1", new { x = 1 } },
                    { "K2", new { x = 2 } },
                }
            };

            string expected = @"
<div>1</div>
<div>2</div>
";

            var template = new RazorEngine().Compile(@"
<div>@Model.Dictionary[""K1""].x</div>
<div>@Model.Dictionary[""K2""].x</div>
");

            // Act
            string actual = template.Run(model);

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void Compile_Returns_GivenFunction()
        {
            // Arrange
            var template = new RazorEngine().Compile(@"
@<area>
    @{ RecursionTest(3); }
</area>

@{

void RecursionTest(int level)
{
	if (level <= 0)
	{
		return;
	}
		
    <div>LEVEL: @level</div>
	@{ RecursionTest(level - 1); }
}

}");

            string expected = @"
<area>
    <div>LEVEL: 3</div>
    <div>LEVEL: 2</div>
    <div>LEVEL: 1</div>
</area>
";

            // Act
            string actual = template.Run();

            // Assert
            actual.Trim().Should().Be(expected.Trim());
        }

        [Fact]
        public void Compile_Returns_GivenTypedModelAction()
        {
            // Arrange
            var template = new RazorEngine().Compile<TestTemplate1>("Hello @A @B @(A + B) @C @Decorator(\"777\")");

            // Act
            string actual = template.Run(instance =>
            {
                instance.A = 1;
                instance.B = 2;
                instance.C = "Alex";
            });

            // Assert
            actual.Should().Be("Hello 1 2 3 Alex -=777=-");
        }

        [Fact]
        public void Compile_Returns_GivenTypedModelInitialize()
        {
            // Arrange
            var template = new RazorEngine().Compile<TestTemplate2>("Hello @Model.Decorator(Model.C)");

            // Act
            string actual = template.Run(instance =>
            {
                instance.Initialize(new TestModel
                {
                    C = "Alex"
                });
            });

            // Assert
            actual.Should().Be("Hello -=Alex=-");
        }

        [Fact]
        public void Compile_Returns_GivenTypedModelConstructor()
        {
            // Arrange
            string templateText = @"
@inherits RazorEngineCore.RazorEngineTemplateBase<RazorEngineCore.Tests.Models.TestModel>
Hello @Model.Decorator(Model.C)
";
            
            var template = new RazorEngine().Compile<RazorEngineTemplateBase<TestModel>>(templateText);

            // Act
            string actual = template.Run(instance =>
            {
                instance.Model = new TestModel
                {
                    C = "Alex"
                };
            });

            // Assert
            actual.Trim().Should().Be("Hello -=Alex=-");
        }

        [Fact]
        public void Compile_Returns_GivenAnonymousObjectWithObjectArray()
        {
            // Arrange
            var template = new RazorEngine().Compile<TestTemplate2>(
@"
@foreach (var item in Model.Numbers.OrderByDescending(x => x))
{
    <p>@item</p>
}
");

            string expected = @"
    <p>3</p>
    <p>2</p>
    <p>1</p>
";

            // Act
            string actual = template.Run(instance =>
            {
                instance.Initialize(new TestModel
                {
                    Numbers = new[] { 2, 1, 3 }
                });
            });

            // Assert
            actual.Should().Be(expected);
        }


        [Fact]
        public void Compile_Returns_GivenTypedLinqModel()
        {
            // Arrange
            var template = new RazorEngine().Compile<TestTemplate2>(
@"
@foreach (var item in Model.Numbers.OrderByDescending(x => x))
{
    <p>@item</p>
}
");
            string expected = @"
    <p>3</p>
    <p>2</p>
    <p>1</p>
";

            // Act
            string actual = template.Run(instance =>
            {
                instance.Initialize(new TestModel
                {
                    Numbers = new[] { 2, 1, 3 }
                });
            });

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void Compile_Returns_GivenDynamicLinqModel()
        {
            // Arrange
            var template = new RazorEngine().Compile(
@"
@foreach (var item in ((IEnumerable<object>)Model.Numbers).OrderByDescending(x => x))
{
    <p>@item</p>
}
");
            string expected = @"
    <p>3</p>
    <p>2</p>
    <p>1</p>
";

            // Act
            string actual = template.Run(new
            {
                    Numbers = new List<object>() {2, 1, 3}
            });

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task Compile_Returns_GivenMetadataReference()
        {
            // Arrange
            string greetingClass = @"
namespace TestAssembly
{
    public static class Greeting
    {
        public static string GetGreeting(string name)
        {
            return ""Hello, "" + name + ""!"";
        }
    }
}
";

            string expected = @"
<p>Hello, Name!</p>
";

            // This needs to be done in the builder to have access to all of the assemblies added through
            // the various AddAssemblyReference options
            CSharpCompilation compilation = CSharpCompilation.Create(
                    "TestAssembly",
                    new[]
                    {
                            CSharpSyntaxTree.ParseText(greetingClass)
                    },
                    GetMetadataReferences(),
                    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );

            MemoryStream memoryStream = new MemoryStream();
            EmitResult emitResult = compilation.Emit(memoryStream);

            if (!emitResult.Success)
            {
                Assert.Fail("Unable to compile test assembly");
            }

            memoryStream.Position = 0;

            // Add an assembly resolver so the assembly can be found
            AppDomain.CurrentDomain.AssemblyResolve += (sender, eventArgs) =>
                    new AssemblyName(eventArgs.Name ?? string.Empty).Name == "TestAssembly"
                            ? Assembly.Load(memoryStream.ToArray())
                            : null;

            var template = await new RazorEngine().CompileAsync(@"
@using TestAssembly
<p>@Greeting.GetGreeting(""Name"")</p>
", builder =>
            {
                builder.AddMetadataReference(MetadataReference.CreateFromStream(memoryStream));

            });

            // Act
            string actual = await template.RunAsync();

            // Assert
            actual.Should().Be(expected);
        }

        private static List<MetadataReference> GetMetadataReferences()
        {
            if (RuntimeInformation.FrameworkDescription.StartsWith(
                ".NET Framework",
                StringComparison.OrdinalIgnoreCase))
            {
                return new List<MetadataReference>()
                           {
                               MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                               MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")).Location),
                               MetadataReference.CreateFromFile(Assembly.Load(
                                   new AssemblyName(
                                       "netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51")).Location),
                               MetadataReference.CreateFromFile(typeof(System.Runtime.GCSettings).Assembly.Location)
                           };
            }

            return new List<MetadataReference>()
                       {
                           MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                           MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("Microsoft.CSharp")).Location),
                           MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("netstandard")).Location),
                           MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Runtime")).Location)
                       };
        }
    }
}
