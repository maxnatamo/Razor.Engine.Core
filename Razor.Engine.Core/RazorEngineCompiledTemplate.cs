using System.Reflection;

namespace RazorEngineCore
{
    public class RazorEngineCompiledTemplate<T> : RazorEngineCompiledTemplate where T : RazorEngineTemplateBase
    {
        /// <inheritdoc />
        protected internal RazorEngineCompiledTemplate(MemoryStream assemblyByteCode, string templateName) : base(assemblyByteCode, templateName)
        { }

        public string Run(Action<T> initializer)
            => this.RunAsync(initializer).GetAwaiter().GetResult();

        public async Task<string> RunAsync(Action<T> initializer)
        {
            T? instance = (T?) Activator.CreateInstance(this.TemplateType);
            if(instance == null)
            {
                throw new OutOfMemoryException($"Failed to allocate type {this.TemplateType.Name}");
            }

            initializer(instance);

            await instance.ExecuteAsync();
            return instance.Result();
        }
    }

    /// <summary>
    /// Representation of a compiled Razor template assembly.
    /// </summary>
    public class RazorEngineCompiledTemplate
    {
        /// <summary>
        /// <see cref="MemoryStream" />-instance containing the binary code for the compiled assembly.
        /// </summary>
        protected MemoryStream AssemblyByteCode { get; set; }

        /// <summary>
        /// The <see cref="Type" /> of the compiled template.
        /// </summary>
        protected Type TemplateType { get; set; }

        /// <summary>
        /// Initialize a new <see cref="RazorEngineCompiledTemplate" />-instance.
        /// </summary>
        /// <param name="assemblyByteCode"><see cref="MemoryStream" />-instance containing the binary code for the compiled assembly.</param>
        /// <param name="templateName">The full name of the compiled template, including namespace.</param>
        /// <exception cref="BadImageFormatException">Thrown if <paramref name="assemblyByteCode" /> doesn't contain valid assembly byte code.</exception>
        /// <exception cref="InvalidDataException">
        /// Thrown if the passed assembly code doesn't contain a type, with the name of <paramref name="templateName" />.
        /// </exception>
        protected internal RazorEngineCompiledTemplate(MemoryStream assemblyByteCode, string templateName)
        {
            this.AssemblyByteCode = assemblyByteCode;

            Assembly assembly = Assembly.Load(assemblyByteCode.ToArray());
            this.TemplateType = assembly.GetType(templateName) ?? throw new InvalidDataException($"Template type not found: {templateName}");
        }

        /// <summary>
        /// Loads a compiled Razor template from a file.
        /// </summary>
        /// <param name="filePath">Path to the file containing the compiled Razor template.</param>
        /// <param name="templateName">The name of the template type in the assembly.</param>
        /// <returns>A read <see cref="RazorEngineCompiledTemplate" />-instance.</returns>
        public static RazorEngineCompiledTemplate LoadFromFile(string filePath, string templateName)
        {
            MemoryStream memoryStream = new MemoryStream();

            using(FileStream fileStream = new FileStream(path: filePath, mode: FileMode.Open))
            {
                fileStream.CopyTo(memoryStream);
            }

            return new RazorEngineCompiledTemplate(memoryStream, templateName);
        }

        /// <summary>
        /// Loads a compiled Razor template from a file.
        /// </summary>
        /// <param name="filePath">Path to the file containing the compiled Razor template.</param>
        /// <param name="templateName">The name of the template type in the assembly.</param>
        /// <returns>A <see cref="Task" />, resolving to the read <see cref="RazorEngineCompiledTemplate" />-instance.</returns>
        public static async Task<RazorEngineCompiledTemplate> LoadFromFileAsync(string filePath, string templateName)
        {
            MemoryStream memoryStream = new MemoryStream();

            using(FileStream fileStream = new FileStream(path: filePath, mode: FileMode.Open))
            {
                await fileStream.CopyToAsync(memoryStream);
            }

            return new RazorEngineCompiledTemplate(memoryStream, templateName);
        }

        /// <summary>
        /// Loads a compiled Razor template from a <see cref="Stream" />.
        /// </summary>
        /// <param name="stream">A <see cref="Stream" />-instance containing the compiled Razor template.</param>
        /// <param name="templateName">The name of the template type in the assembly.</param>
        /// <returns>A read <see cref="RazorEngineCompiledTemplate" />-instance.</returns>
        public static RazorEngineCompiledTemplate LoadFromStream(Stream stream, string templateName)
        {
            MemoryStream memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            memoryStream.Position = 0;

            return new RazorEngineCompiledTemplate(memoryStream, templateName);
        }

        /// <summary>
        /// Loads a compiled Razor template from a <see cref="Stream" />.
        /// </summary>
        /// <param name="stream">A <see cref="Stream" />-instance containing the compiled Razor template.</param>
        /// <param name="templateName">The name of the template type in the assembly.</param>
        /// <returns>A <see cref="Task" />, resolving to the read <see cref="RazorEngineCompiledTemplate" />-instance.</returns>
        public static async Task<RazorEngineCompiledTemplate> LoadFromStreamAsync(Stream stream, string templateName)
        {
            MemoryStream memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            return new RazorEngineCompiledTemplate(memoryStream, templateName);
        }

        /// <summary>
        /// Saves the Razor template to a <see cref="Stream" />.
        /// </summary>
        /// <param name="stream">The <see cref="Stream" />-instance to write the template to.</param>
        public void SaveToStream(Stream stream)
        {
            this.AssemblyByteCode.CopyTo(stream);
        }

        /// <summary>
        /// Saves the Razor template to a <see cref="Stream" />.
        /// </summary>
        /// <param name="stream">The <see cref="Stream" />-instance to write the template to.</param>
        /// <returns>A <see cref="Task" /> representing the copying of the stream.</returns>
        public async Task SaveToStreamAsync(Stream stream)
        {
            await this.AssemblyByteCode.CopyToAsync(stream);
        }

        /// <summary>
        /// Saves the Razor template to a file.
        /// </summary>
        /// <param name="filePath">Path to the file, to which the compiled Razor template should be saved to.</param>
        public void SaveToFile(string filePath)
        {
            using(FileStream fileStream = new FileStream(path: filePath, mode: FileMode.OpenOrCreate))
            {
                AssemblyByteCode.CopyTo(fileStream);
            }
        }

        /// <summary>
        /// Saves the Razor template to a file.
        /// </summary>
        /// <param name="filePath">Path to the file, to which the compiled Razor template should be saved to.</param>
        /// <returns>A <see cref="Task" /> representing the copying of the stream.</returns>
        public async Task SaveToFileAsync(string filePath)
        {
            using(FileStream fileStream = new FileStream(path: filePath, mode: FileMode.OpenOrCreate))
            {
                await AssemblyByteCode.CopyToAsync(fileStream);
            }
        }

        /// <summary>
        /// Execute the template, optionally, with the specified <paramref name="model" /> as a model.
        /// </summary>
        /// <param name="model">The optional model to pass to the view.</param>
        /// <returns>A <see cref="string" /> containing the compiled and executed view.</returns>
        public string Run(object? model = null)
        {
            return this.RunAsync(model).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Execute the template, optionally, with the specified <paramref name="model" /> as a model.
        /// </summary>
        /// <param name="model">The optional model to pass to the view.</param>
        /// <returns>A <see cref="Task" />, resolving to a <see cref="string" /> containing the compiled and executed view.</returns>
        public async Task<string> RunAsync(object? model = null)
        {
            if(model != null && model.IsAnonymous())
            {
                model = new AnonymousTypeWrapper(model);
            }

            RazorEngineTemplateBase? instance = (RazorEngineTemplateBase?) Activator.CreateInstance(this.TemplateType);
            if(instance == null)
            {
                throw new OutOfMemoryException($"Failed to create instance of type {this.TemplateType.Name}");
            }

            instance.Model = model;

            await instance.ExecuteAsync();

            return instance.Result();
        }
    }
}