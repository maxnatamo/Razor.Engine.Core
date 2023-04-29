using System.Reflection;
using Microsoft.CodeAnalysis;

namespace RazorEngineCore
{
    /// <summary>
    /// Options for compilation with <see cref="RazorEngine" />.
    /// </summary>
    public class RazorEngineCompilationOptions
    {
        /// <summary>
        /// The namespace of the compiled Razor template type.
        /// </summary>
        public string TemplateNamespace { get; set; } = "TemplateNamespace";

        /// <summary>
        /// The name of the compiled Razor template type.
        /// </summary>
        public string TemplateTypeName { get; set; } = "Template";

        /// <summary>
        /// Full name of the compiled Razor template type.
        /// </summary>
        public string TemplateTypeFullName => $"{TemplateNamespace}.{TemplateTypeName}";

        /// <summary>
        /// The file path for the resulting template assembly file.
        /// </summary>
        public string TemplateFilename { get; set; } = "";

        /// <summary>
        /// The name of the type that the template should inherit from.
        /// </summary>
        public string Inherits { get; set; } = typeof(RazorEngineTemplateBase).FullName ?? string.Empty;

        /// <summary>
        /// Set of referenced assemblies during compilation.
        /// </summary>
        public HashSet<Assembly> ReferencedAssemblies { get; set; } = new HashSet<Assembly>();

        /// <summary>
        /// Set of <see cref="MetadataReference" /> to use during compilation.
        /// </summary>
        public HashSet<MetadataReference> MetadataReferences { get; set; } = new HashSet<MetadataReference>();

        /// <summary>
        /// Set of default namespace imports.
        /// </summary>
        public HashSet<string> DefaultUsings { get; set; } = new HashSet<string>()
        {
            "System.Linq",
            "System.Collections",
            "System.Collections.Generic"
        };

        /// <summary>
        /// Initialize new <see cref="RazorEngineCompilationOptions" />-instance.
        /// </summary>
        public RazorEngineCompilationOptions()
        {
            this.ReferencedAssemblies = new HashSet<Assembly>()
            {
                Assembly.Load("System.Linq"),
                Assembly.Load("System.Linq.Expressions"),
                Assembly.Load("System.Runtime"),
                Assembly.Load("System.Collections"),
                Assembly.Load("Microsoft.CSharp"),
                typeof(object).Assembly,
                typeof(System.Collections.IList).Assembly,
                typeof(System.Collections.Generic.IEnumerable<>).Assembly,
                typeof(RazorEngineTemplateBase).Assembly,
            };
        }
    }
}