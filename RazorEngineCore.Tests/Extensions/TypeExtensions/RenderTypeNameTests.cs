using RazorEngineCore.Tests.Models;

namespace RazorEngineCore.Tests.Extensions.TypeExtensionsTests
{
    public class RenderTypeNameTests
    {
        [Fact]
        public void RenderTypeName_ReturnsName_GivenNonGenericType()
        {
            // Arrange
            Type type = typeof(Item);

            // Act
            string typeName = type.RenderTypeName();

            // Assert
            typeName.Should().Be("RazorEngineCore.Tests.Models.Item");
        }

        [Fact]
        public void RenderTypeName_ReturnsName_GivenGenericType()
        {
            // Arrange
            Type type = typeof(GenericModel<int>);

            // Act
            string typeName = type.RenderTypeName();

            // Assert
            typeName.Should().Be("RazorEngineCore.Tests.Models.GenericModel<System.Int32>");
        }

        [Fact]
        public void RenderTypeName_ReturnsName_GivenNestedGenericType()
        {
            // Arrange
            Type type = typeof(GenericModel<GenericModel<int>>);

            // Act
            string typeName = type.RenderTypeName();

            // Assert
            typeName.Should().Be("RazorEngineCore.Tests.Models.GenericModel<RazorEngineCore.Tests.Models.GenericModel<System.Int32>>");
        }

        [Fact]
        public void RenderTypeName_ReturnsName_GivenAnonymousObject()
        {
            // Arrange
            Object obj = new Object { };
            Type type = obj.GetType();

            // Act
            string typeName = type.RenderTypeName();

            // Assert
            typeName.Should().Be("System.Object");
        }

        [Fact]
        public void RenderTypeName_ReturnsNestedName_GivenNestedModel()
        {
            // Arrange
            Type type = typeof(NestedTestModel.TestModelInnerClass1.TestModelInnerClass2);

            // Act
            string typeName = type.RenderTypeName();

            // Assert
            typeName.Should().Be("RazorEngineCore.Tests.Models.NestedTestModel.TestModelInnerClass1.TestModelInnerClass2");
        }

        [Fact]
        public void RenderTypeName_ReturnsNameWithoutNamespace_GivenModelWithoutNamespace()
        {
            // Arrange
            Type type = typeof(TestModelWithoutNamespace);

            // Act
            string typeName = type.RenderTypeName();

            // Assert
            typeName.Should().Be("TestModelWithoutNamespace");
        }

        [Fact]
        public void RenderTypeName_ReturnsNameWithoutNamespace_GivenModelWithoutIncludingNamespace()
        {
            // Arrange
            Type type = typeof(Item);

            // Act
            string typeName = type.RenderTypeName(includeNamespace: false);

            // Assert
            typeName.Should().Be("Item");
        }
    }
}