using System.Reflection.Metadata;
using System.Text;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace RazorEngineCore
{
    /// <summary>
    /// Engine for compiling and executing Razor templates.
    /// </summary>
    public class RazorEngine
    {
        /// <summary>
        /// Compile the specified <paramref name="content" /> as a Razor template and return it.
        /// </summary>
        /// <param name="content">The Razor template content to compile.</param>
        /// <param name="builderAction">Action for defining options for the compilation.</param>
        /// <typeparam name="T">The base type for the template.</typeparam>
        /// <returns>The compiled <see cref="RazorEngineCompiledTemplate" /> template instance.</returns>
        /// <exception cref="RazorEngineCompilationException">Thrown if compilation of the template source code failed.<exception>
        public RazorEngineCompiledTemplate<T> Compile<T>(string content, Action<RazorEngineCompilationOptions>? builderAction = null) where T : RazorEngineTemplateBase
        {
            RazorEngineCompilationOptions compilationOptions = new RazorEngineCompilationOptions();
            compilationOptions.SetInherits(typeof(T));

            builderAction?.Invoke(compilationOptions);

            MemoryStream memoryStream = this.CreateAndCompileToStream(content, compilationOptions);
            return new RazorEngineCompiledTemplate<T>(memoryStream, compilationOptions.TemplateTypeFullName);
        }

        /// <summary>
        /// Compile the specified <paramref name="content" /> as a Razor template and return it, asynchronously.
        /// </summary>
        /// <param name="content">The Razor template content to compile.</param>
        /// <param name="builderAction">Action for defining options for the compilation.</param>
        /// <typeparam name="T">The base type for the template.</typeparam>
        /// <returns>The compiled <see cref="RazorEngineCompiledTemplate" /> template instance.</returns>
        /// <exception cref="RazorEngineCompilationException">Thrown if compilation of the template source code failed.<exception>
        public async Task<RazorEngineCompiledTemplate<T>> CompileAsync<T>(string content, Action<RazorEngineCompilationOptions>? builderAction = null) where T : RazorEngineTemplateBase
        {
            return await Task.Run(() => this.Compile<T>(content: content, builderAction: builderAction));
        }

        /// <summary>
        /// Compile the specified <paramref name="content" /> as a Razor template and return it.
        /// </summary>
        /// <param name="content">The Razor template content to compile.</param>
        /// <param name="builderAction">Action for defining options for the compilation.</param>
        /// <returns>The compiled <see cref="RazorEngineCompiledTemplate" /> template instance.</returns>
        /// <exception cref="RazorEngineCompilationException">Thrown if compilation of the template source code failed.<exception>
        public RazorEngineCompiledTemplate Compile(string content, Action<RazorEngineCompilationOptions>? builderAction = null)
        {
            RazorEngineCompilationOptions compilationOptions = new RazorEngineCompilationOptions();
            compilationOptions.SetInherits(typeof(RazorEngineTemplateBase));

            builderAction?.Invoke(compilationOptions);

            MemoryStream memoryStream = this.CreateAndCompileToStream(content, compilationOptions);
            return new RazorEngineCompiledTemplate(memoryStream, compilationOptions.TemplateTypeFullName);
        }


        /// <summary>
        /// Compile the specified <paramref name="content" /> as a Razor template and return it, asynchronously.
        /// </summary>
        /// <param name="content">The Razor template content to compile.</param>
        /// <param name="builderAction">Action for defining options for the compilation.</param>
        /// <returns>The compiled <see cref="RazorEngineCompiledTemplate" /> template instance.</returns>
        /// <exception cref="RazorEngineCompilationException">Thrown if compilation of the template source code failed.<exception>
        public async Task<RazorEngineCompiledTemplate> CompileAsync(string content, Action<RazorEngineCompilationOptions>? builderAction = null)
        {
            return await Task.Run(() => this.Compile(content: content, builderAction: builderAction));
        }

        /// <summary>
        /// Compile the specified <paramref name="templateSource" /> source code into an assembly, containing the resulting template type.
        /// </summary>
        /// <param name="templateSource">The Razor template content to compile.</param>
        /// <param name="options">Action for defining options for the compilation.</param>s
        /// <returns>A <see cref="MemoryStream" />-instance, containing the resulting assembly.</returns>
        /// <exception cref="RazorEngineCompilationException">Thrown if compilation of the template source code failed.<exception>
        protected virtual MemoryStream CreateAndCompileToStream(string templateSource, RazorEngineCompilationOptions options)
        {
            templateSource = this.WriteDirectives(templateSource, options);

            RazorProjectEngine engine = RazorProjectEngine.Create(
                RazorConfiguration.Default,
                RazorProjectFileSystem.Create(@"."),
                (builder) =>
                {
                    builder.SetNamespace(options.TemplateNamespace);
                });

            string fileName = string.IsNullOrWhiteSpace(options.TemplateFilename) ? Path.GetRandomFileName() : options.TemplateFilename;

            RazorSourceDocument document = RazorSourceDocument.Create(templateSource, fileName);
            RazorCodeDocument codeDocument = engine.Process(
                document,
                null,
                new List<RazorSourceDocument>(),
                new List<TagHelperDescriptor>());

            RazorCSharpDocument razorCSharpDocument = codeDocument.GetCSharpDocument();
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(razorCSharpDocument.GeneratedCode);

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName: fileName,
                syntaxTrees: new[]
                {
                    syntaxTree
                },
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                references: options.ReferencedAssemblies
                   .Select(assembly =>
                   {
                       unsafe
                       {
                           assembly.TryGetRawMetadata(out byte* blob, out int length);
                           ModuleMetadata moduleMetadata = ModuleMetadata.CreateFromMetadata((IntPtr) blob, length);
                           AssemblyMetadata assemblyMetadata = AssemblyMetadata.Create(moduleMetadata);
                           PortableExecutableReference metadataReference = assemblyMetadata.GetReference();

                           return metadataReference;
                       }
                   })
                   .Concat(options.MetadataReferences).ToList());

            MemoryStream memoryStream = new MemoryStream();
            EmitResult emitResult = compilation.Emit(memoryStream);

            if(!emitResult.Success)
            {
                throw new RazorEngineCompilationException()
                {
                    Errors = emitResult.Diagnostics.ToList(),
                    GeneratedCode = razorCSharpDocument.GeneratedCode
                };
            }

            memoryStream.Position = 0;

            return memoryStream;
        }

        /// <summary>
        /// Prepends the directives defined in <paramref name="options" /> to <paramref name="content" /> and returns it.
        /// </summary>
        /// <param name="content">The content of the Razor template.</param>
        /// <param name="options">The <see cref="RazorEngineCompilationOptions" />-instance, containing the directives.</param>
        /// <returns>The content with directives added.</returns>
        protected virtual string WriteDirectives(string content, RazorEngineCompilationOptions options)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"@inherits {options.Inherits}");

            foreach(string entry in options.DefaultUsings)
            {
                stringBuilder.AppendLine($"@using {entry}");
            }

            stringBuilder.Append(content);

            return stringBuilder.ToString();
        }
    }
}