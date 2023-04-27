namespace RazorEngineCore.Tests.Models
{
    public class TestModel
    {
        public int A { get; set; }
        public int B { get; set; }
        public string C { get; set; } = string.Empty;
        public DateTime D { get; set; }
        public IList<int> Numbers { get; set; } = new List<int>();
        public IList<object> Objects { get; set; } = new List<object>();
        public DateTime? DateTime { get; set; }

        public string Decorator(string text)
        {
            return "-=" + text + "=-";
        }
    }
}