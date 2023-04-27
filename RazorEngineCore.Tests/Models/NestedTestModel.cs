namespace RazorEngineCore.Tests.Models
{
    public class NestedTestModel
    {
        public string Name { get; set; } = string.Empty;
        public int[] Items { get; set; } = new int[] {};

        public class TestModelInnerClass1
        {
            public string Name { get; set; } = string.Empty;
            public int[] Items { get; set; } = new int[] {};

            public class TestModelInnerClass2
            {
                public string Name { get; set; } = string.Empty;
                public int[] Items { get; set; } = new int[] {};
            }
        }
    }
}
