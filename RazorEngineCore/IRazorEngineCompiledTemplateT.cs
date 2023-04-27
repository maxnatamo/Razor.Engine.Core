namespace RazorEngineCore
{
    public interface IRazorEngineCompiledTemplate<out T> where T : IRazorEngineTemplate
    {
        void SaveToStream(Stream stream);
        Task SaveToStreamAsync(Stream stream);
        void SaveToFile(string fileName);
        Task SaveToFileAsync(string fileName);
        string Run(Action<T> initializer);
        Task<string> RunAsync(Action<T> initializer);
    }
}