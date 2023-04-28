using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;

namespace RazorEngineCore
{
    public class RazorEngineCompilationOptions
    {
        public HashSet<Assembly> ReferencedAssemblies { get; set; } = new HashSet<Assembly>();

        public HashSet<MetadataReference> MetadataReferences { get; set; } = new HashSet<MetadataReference>();
        public string TemplateNamespace { get; set; } = "TemplateNamespace";
        public string TemplateFilename { get; set; } = "";
        public string Inherits { get; set; } = "RazorEngineCore.RazorEngineTemplateBase";

        public HashSet<string> DefaultUsings { get; set; } = new HashSet<string>()
        {
            "System.Linq",
            "System.Collections",
            "System.Collections.Generic"
        };

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