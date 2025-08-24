using Jim.Mddr.Extensions;

namespace Jim.Mddr.Test.Extensions.EnumerableExtensions;

public class When_HasNoElements
{
    [Fact]
    public void Should_Return_True_When_Collection_Has_No_Elements()
    {
        // Arrange
        var collection = new List<int>();

        // Act
        var result = collection.HasNoElements();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Should_Return_False_When_Collection_Has_Elements()
    {
        // Arrange
        var collection = new List<int> { 1, 2, 3 };

        // Act
        var result = collection.HasNoElements();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Should_Return_False_When_Collection_Is_Null()
    {
        // Arrange
        List<int> collection = null;

        // Act
        var result = collection.HasNoElements();

        // Assert
        Assert.False(result);
    }
}
