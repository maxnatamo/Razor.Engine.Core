using System.Reflection;

namespace RazorEngineCore
{
    public class RazorEngineCompiledTemplate<T> where T : RazorEngineTemplateBase
    {
        protected MemoryStream assemblyByteCode { get; set; }
        protected Type templateType { get; set; }

        internal RazorEngineCompiledTemplate(MemoryStream assemblyByteCode, string templateNamespace)
        {
            this.assemblyByteCode = assemblyByteCode;

            Assembly assembly = Assembly.Load(assemblyByteCode.ToArray());
            this.templateType = assembly.GetType($"{templateNamespace}.Template") ?? throw new InvalidDataException();
        }

        public static RazorEngineCompiledTemplate<T> LoadFromFile(string fileName, string templateNamespace = "TemplateNamespace")
        {
            return LoadFromFileAsync(fileName: fileName, templateNamespace: templateNamespace).GetAwaiter().GetResult();
        }

        public static async Task<RazorEngineCompiledTemplate<T>> LoadFromFileAsync(string fileName, string templateNamespace = "TemplateNamespace")
        {
            MemoryStream memoryStream = new MemoryStream();

            using(FileStream fileStream = new FileStream(
                path: fileName,
                mode: FileMode.Open,
                access: FileAccess.Read,
                share: FileShare.None,
                bufferSize: 4096,
                useAsync: true))
            {
                await fileStream.CopyToAsync(memoryStream);
            }

            return new RazorEngineCompiledTemplate<T>(memoryStream, templateNamespace);
        }

        public static RazorEngineCompiledTemplate<T> LoadFromStream(Stream stream)
        {
            return LoadFromStreamAsync(stream).GetAwaiter().GetResult();
        }

        public static async Task<RazorEngineCompiledTemplate<T>> LoadFromStreamAsync(Stream stream, string templateNamespace = "TemplateNamespace")
        {
            MemoryStream memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            return new RazorEngineCompiledTemplate<T>(memoryStream, templateNamespace);
        }

        public void SaveToStream(Stream stream)
        {
            this.SaveToStreamAsync(stream).GetAwaiter().GetResult();
        }

        public Task SaveToStreamAsync(Stream stream)
        {
            return this.assemblyByteCode.CopyToAsync(stream);
        }

        public void SaveToFile(string fileName)
        {
            this.SaveToFileAsync(fileName).GetAwaiter().GetResult();
        }

        public Task SaveToFileAsync(string fileName)
        {
            using(FileStream fileStream = new FileStream(
                path: fileName,
                mode: FileMode.OpenOrCreate,
                access: FileAccess.Write,
                share: FileShare.None,
                bufferSize: 4096,
                useAsync: true))
            {
                return assemblyByteCode.CopyToAsync(fileStream);
            }
        }

        public string Run(Action<T> initializer)
        {
            return this.RunAsync(initializer).GetAwaiter().GetResult();
        }

        public async Task<string> RunAsync(Action<T> initializer)
        {
            T? instance = (T?) Activator.CreateInstance(this.templateType);

            if(instance == null)
            {
                throw new OutOfMemoryException($"Failed to allocate type {this.templateType.Name}");
            }

            initializer(instance);

            await instance.ExecuteAsync();
            return await instance.ResultAsync();
        }
    }
}