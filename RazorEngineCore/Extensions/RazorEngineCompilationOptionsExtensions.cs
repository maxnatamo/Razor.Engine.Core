using System.Reflection;
using Microsoft.CodeAnalysis;

namespace RazorEngineCore
{
    /// <summary>
    /// Extensions for <see cref="RazorEngineCompilationOptions" />.
    /// </summary>
    public static class RazorEngineCompilationOptionsExtensions
    {
        /// <summary>
        /// Add an assembly reference using an <see cref="Assembly" />-instance.
        /// </summary>
        /// <param name="options">The options to add the reference to.</param>
        /// <param name="assembly">The <see cref="Assembly" />-instance to add as reference.</param>
        /// <returns>The <see cref="RazorEngineCompilationOptions" />, to allow for chaining.</returns>
        public static RazorEngineCompilationOptions AddAssemblyReference(this RazorEngineCompilationOptions options, Assembly assembly)
        {
            options.ReferencedAssemblies.Add(assembly);
            return options;
        }

        /// <summary>
        /// Add an assembly reference by name.
        /// </summary>
        /// <param name="options">The options to add the reference to.</param>
        /// <param name="assemblyName">The name of the assembly to add.</param>
        /// <returns>The <see cref="RazorEngineCompilationOptions" />, to allow for chaining.</returns>
        public static RazorEngineCompilationOptions AddAssemblyReference(this RazorEngineCompilationOptions options, string assemblyName)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            options.AddAssemblyReference(assembly);

            return options;
        }

        /// <summary>
        /// Add an assembly reference by type. This will add the parent assembly of the specified <paramref name="type" />, recursively.
        /// </summary>
        /// <param name="options">The options to add the reference to.</param>
        /// <param name="type">The type to add assemblies from.</param>
        /// <returns>The <see cref="RazorEngineCompilationOptions" />, to allow for chaining.</returns>
        public static RazorEngineCompilationOptions AddAssemblyReference(this RazorEngineCompilationOptions options, Type type)
        {
            options.AddAssemblyReference(type.Assembly);

            foreach(Type argumentType in type.GenericTypeArguments)
            {
                options.AddAssemblyReference(argumentType);
            }

            return options;
        }

        /// <summary>
        /// Add a metadata reference.
        /// </summary>
        /// <param name="options">The options to add the reference to.</param>
        /// <param name="reference">The <see cref="MetadataReference" />-instance to add.</param>
        /// <returns>The <see cref="RazorEngineCompilationOptions" />, to allow for chaining.</returns>
        public static RazorEngineCompilationOptions AddMetadataReference(this RazorEngineCompilationOptions options, MetadataReference reference)
        {
            options.MetadataReferences.Add(reference);
            return options;
        }

        /// <summary>
        /// Add a default namespace to the compilation.
        /// </summary>
        /// <param name="options">The options to add the namespace to.</param>
        /// <param name="namespaceName">The name of the namespace to add.</param>
        /// <returns>The <see cref="RazorEngineCompilationOptions" />, to allow for chaining.</returns>
        public static RazorEngineCompilationOptions AddImport(this RazorEngineCompilationOptions options, string namespaceName)
        {
            options.DefaultUsings.Add(namespaceName);
            return options;
        }

        /// <summary>
        /// Set the type which the compiled template should inherit from.
        /// </summary>
        /// <param name="options">The options to alter.</param>
        /// <param name="inheritedType">The type which the compiled template should inherit from..</param>
        /// <returns>The <see cref="RazorEngineCompilationOptions" />, to allow for chaining.</returns>
        public static RazorEngineCompilationOptions SetInherits(this RazorEngineCompilationOptions options, Type inheritedType)
        {
            options.Inherits = inheritedType.RenderTypeName();
            options.AddAssemblyReference(inheritedType);

            return options;
        }
    }
}