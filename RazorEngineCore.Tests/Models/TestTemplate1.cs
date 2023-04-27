namespace RazorEngineCore.Tests.Models
{
    public class TestTemplate1 : RazorEngineTemplateBase
    {
        public int A { get; set; }
        public int B { get; set; }
        public string C { get; set; } = string.Empty;
        public DateTime D { get; set; }

        public IList<int> Numbers { get; set; } = new List<int>();

        public string Decorator(string text)
        {
            return "-=" + text + "=-";
        }
    }
}