namespace ConsoleShortageManager.Tests;

public class ShortageManagerTests : IAsyncLifetime
{
    [Theory]
    [InlineData(Room.Bathroom)]
    [InlineData(Room.Kitchen)]
    [InlineData(Room.MeetingRoom)]
    public void MatchShortage_ByRoom_True(Room room)
    {
        // Arrange
        var testShortage = new Shortage
        {
            Room = room,
            CreatedBy = UserValidation.GetUser()
        };

        // Act
        var testOutput = ShortageManager.MatchShortage(testShortage, room);

        // Assert
        Assert.True(testOutput);
    }

    [Theory]
    [InlineData(Room.Kitchen)]
    [InlineData(Room.MeetingRoom)]
    public void MatchShortage_ByRoom_False(Room room) 
    {
        // Arrange
        var testShortage = new Shortage
        {
            Room = Room.Bathroom,
            CreatedBy = UserValidation.GetUser(),
        };

        // Act
        var testOutput = ShortageManager.MatchShortage(testShortage, room);

        // Assert
        Assert.False(testOutput);
    }

    [Theory]
    [InlineData(Category.Electronics)]
    [InlineData(Category.Food)]
    [InlineData(Category.Other)]
    public void MatchShortage_ByCategory_True(Category category)
    {
        // Arrange
        var testShortage = new Shortage
        {
            Category = category,
            CreatedBy = UserValidation.GetUser()
        };

        // Act
        var testOutput = ShortageManager.MatchShortage(testShortage, category);

        // Assert
        Assert.True(testOutput);
    }

    [Theory]
    [InlineData(Category.Food)]
    [InlineData(Category.Electronics)]
    public void MatchShortage_ByCategory_False(Category category)
    {
        // Arrange
        var testShortage = new Shortage
        {
            Category = Category.Other,
            CreatedBy = UserValidation.GetUser(),
        };

        // Act
        var testOutput = ShortageManager.MatchShortage(testShortage, category);

        // Assert
        Assert.False(testOutput);
    }

    [Theory]
    [InlineData(Category.Food)]
    [InlineData(Category.Electronics)]
    public void MatchStorage_ByUserValidation_False(Category category)
    {
        // Arrange
        var testShortage = new Shortage
        {
            Category = category,
        };

        // Act
        var testOutput = ShortageManager.MatchShortage(testShortage, category);

        // Assert
        Assert.False(testOutput);
    }

    [Fact]
    public void MatchShortage_AdminValidation_True()
    {
        // Arrange
        UserValidation.InitializeUser();
        User.IsAdmin = true;

        var testShortage = new Shortage
        {
            Category = Category.Electronics,
            CreatedBy = "RandomTestUser"
        };
        
        // Act
        var testOutput = ShortageManager.MatchShortage(testShortage, Category.Electronics);

        // Assert
        Assert.True(testOutput);
    }

    public Task InitializeAsync()
    {
        UserValidation.InitializeUser();
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}