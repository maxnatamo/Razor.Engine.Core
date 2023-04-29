using System.Runtime.Serialization;
using Microsoft.CodeAnalysis;

namespace RazorEngineCore
{
    [Serializable]
    public class RazorEngineCompilationException : RazorEngineException
    {
        public List<Diagnostic> Errors { get; set; } = new List<Diagnostic>();

        public string GeneratedCode { get; set; } = string.Empty;

        public override string Message
        {
            get
            {
                string errors = string.Join("\n", this.Errors.Where(w => w.IsWarningAsError || w.Severity == DiagnosticSeverity.Error));
                return "Unable to compile template:\n" + errors;
            }
        }

        public RazorEngineCompilationException()
        {

        }

        protected RazorEngineCompilationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        public RazorEngineCompilationException(Exception innerException) : base(string.Empty, innerException)
        {

        }
    }
}