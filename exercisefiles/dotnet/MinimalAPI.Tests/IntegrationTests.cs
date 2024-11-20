using System.Net;
using System.Text;
using System.Net.Http.Headers;

namespace IntegrationTests;

public class IntegrationTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public IntegrationTests(TestWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnsHelloWorld()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Hello World!", content);
    }

    [Fact]
    public async Task Get_ReturnsNotFound()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/nonexistent");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_ReturnsCreated()
    {
        // Arrange
        var data = new StringContent("{\"name\":\"test\"}", Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/create", data);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Put_ReturnsNoContent()
    {
        // Arrange
        var data = new StringContent("{\"name\":\"updated\"}", Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/update/1", data);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent()
    {
        // Arrange

        // Act
        var response = await _client.DeleteAsync("/delete/1");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
