<h1 align="center">
  🪒 Razor.Engine.Core
</h1>

> Compile and execute Razor templates outside MVC applications.

<div align="center">
  <a href="https://github.com/maxnatamo/RazorEngineCore/blob/main/LICENSE">
    <img src="https://img.shields.io/github/license/maxnatamo/RazorEngineCore?style=for-the-badge" />
  </a>
  <a href="https://github.com/maxnatamo/RazorEngineCore/blob/main/CONTRIBUTING.md">
    <img src="https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=for-the-badge" />
  </a>
  <br />
  <a href="https://github.com/maxnatamo/RazorEngineCore/actions">
    <img src="https://img.shields.io/github/actions/workflow/status/maxnatamo/RazorEngineCore/continuous.yml?branch=main&label=Build&style=for-the-badge" />
  </a>
  <a href="https://www.nuget.org/packages/Razor.Engine.Core/">
    <img src="https://img.shields.io/nuget/v/Razor.Engine.Core?label=Dev&style=for-the-badge" />
  </a>
  <a href="https://www.nuget.org/packages/Razor.Engine.Core/">
    <img src="https://img.shields.io/nuget/v/Razor.Engine.Core?label=PROD&style=for-the-badge" />
  </a>
</div>

Features:
* .NET 7.0
* .NET 6.0
* Windows / Linux
* Publish as single file supported
* Thread safe

Every single star makes maintainer happy! ⭐

## 🧯 Installation

To use RazorEngineCore in a project, install the package with NuGet:

```sh
dotnet new install Razor.Engine.Core
```

## Examples

#### Basic usage

```cs
RazorEngine razorEngine = new RazorEngine();
RazorEngineCompiledTemplate template = razorEngine.Compile("Hello @Model.Name");

string result = template.Run(new
{
    Name = "Alexander"
});

Console.WriteLine(result);
```

#### Strongly typed model

```cs
RazorEngine razorEngine = new RazorEngine();
string templateText = "Hello @Model.Name";

// yeah, heavy definition
RazorEngineCompiledTemplate<RazorEngineTemplateBase<TestModel>> template = razorEngine.Compile<RazorEngineTemplateBase<TestModel>>(templateText);

string result = template.Run(instance =>
{
    instance.Model = new TestModel()
    {
        Name = "Hello",
        Items = new[] {3, 1, 2}
    };
});

Console.WriteLine(result);
```

#### Save / Load compiled templates

Most expensive task is to compile template, you should not compile template every time you need to run it
```cs
RazorEngine razorEngine = new RazorEngine();
RazorEngineCompiledTemplate template = razorEngine.Compile("Hello @Model.Name");

// Save to file
template.SaveToFile("myTemplate.dll");

// Save to stream
MemoryStream memoryStream = new MemoryStream();
template.SaveToStream(memoryStream);
```

```cs
RazorEngineCompiledTemplate template1 = RazorEngineCompiledTemplate.LoadFromFile("myTemplate.dll");
RazorEngineCompiledTemplate template2 = RazorEngineCompiledTemplate.LoadFromStream(myStream);
```

```cs
RazorEngineCompiledTemplate<MyBase> template1 = RazorEngineCompiledTemplate<MyBase>.LoadFromFile<MyBase>("myTemplate.dll");
RazorEngineCompiledTemplate<MyBase> template2 = RazorEngineCompiledTemplate<MyBase>.LoadFromStream<MyBase>(myStream);
```

#### Caching
RazorEngineCore is not responsible for caching. Each team and project has their own caching frameworks and conventions therefore making it impossible to have builtin solution for all possible needs. 

If you dont have one, use following static ConcurrentDictionary example as a simplest thread safe solution.

```cs
private static ConcurrentDictionary<int, RazorEngineCompiledTemplate> TemplateCache = new ConcurrentDictionary<int, RazorEngineCompiledTemplate>();
```

```cs
private string RenderTemplate(string template, object model)
{
    int hashCode = template.GetHashCode();

    RazorEngineCompiledTemplate compiledTemplate = TemplateCache.GetOrAdd(hashCode, i =>
    {
        RazorEngine razorEngine = new RazorEngine();
        return razorEngine.Compile(Content);
    });

    return compiledTemplate.Run(model);
}
```

#### Template functions

ASP.NET Core way of defining template functions:
```
<area>
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
}
```

Output:
```
<div>LEVEL: 3</div>
<div>LEVEL: 2</div>
<div>LEVEL: 1</div>
```

#### Helpers and custom members

```cs
string content = @"Hello @A, @B, @Decorator(123)";

RazorEngine razorEngine = new RazorEngine();
RazorEngineCompiledTemplate<CustomTemplate> template = razorEngine.Compile<CustomTemplate>(content);

string result = template.Run(instance =>
{
    instance.A = 10;
    instance.B = "Alex";
});

Console.WriteLine(result);
```

```cs
public class CustomTemplate : RazorEngineTemplateBase
{
    public int A { get; set; }
    public string B { get; set; }

    public string Decorator(object value)
    {
        return "-=" + value + "=-";
    }
}
```

#### Referencing assemblies

Keep your templates as simple as possible, if you need to inject "unusual" assemblies most likely you are doing it wrong.
Writing `@using System.IO` in template will not reference System.IO assembly, use builder to manually reference it.

```cs
RazorEngine razorEngine = new RazorEngine();
RazorEngineCompiledTemplate compiledTemplate = razorEngine.Compile(templateText, builder =>
{
    builder.AddAssemblyReferenceByName("System.Security"); // by name
    builder.AddAssemblyReference(typeof(System.IO.File)); // by type
    builder.AddAssemblyReference(Assembly.Load("source")); // by reference
});

string result = compiledTemplate.Run(new { name = "Hello" });
```

## Extensions
* [wdcossey/RazorEngineCore.Extensions](https://github.com/wdcossey/RazorEngineCore.Extensions)
	- HTML values encoded by default
	- Template precompiling
	- Direct model usage without RazorEngineTemplateBase
	```cs
	template.Run(object model = null)
	template.RunAsync(object model = null)
	template.Run<TModel>(TModel model = null)
	template.RunAsync<TModel>(TModel model = null)
	```

## 📝 Contributing

If you want to contribute, great! We'd love your help!

For more in-depth information on contributing to the project and how to get started, see [CONTRIBUTING](CONTRIBUTING.md).