using Microsoft.AspNetCore.Mvc.Testing;

namespace LtPhotoAlbum.Tests;

public class ProgramTests : IClassFixture<WebApplicationFactory<Program>>
{
    readonly WebApplicationFactory<Program> _factory;

    public ProgramTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task HomepageShouldReturnLuckyButton()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("data-testid='luckyBtn'", content);
    }

    [Fact]
    public async Task WhenAlbumIdIsNotNull_HomepageShouldReturnLuckyButtonAndPhotos()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("123");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("data-testid='luckyBtn'", content);
        Assert.Contains("data-testid='photo'", content);
    }
}