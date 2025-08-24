namespace Jim.Mddr.Extensions.Tests;

public class When_HasElements
{
    [Fact]
    public void Should_Return_True_When_Collection_Has_Elements()
    {
        // Arrange
        var collection = new List<int> { 1, 2, 3 };

        // Act
        var result = collection.HasElements();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Should_Return_False_When_Collection_Is_Empty()
    {
        // Arrange
        var collection = new List<int>();

        // Act
        var result = collection.HasElements();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Should_Return_False_When_Collection_Is_Null()
    {
        // Arrange
        List<int> collection = null;

        // Act
        var result = collection.HasElements();

        // Assert
        Assert.False(result);
    }
}