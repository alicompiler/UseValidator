namespace Lib.Tests.Integration;

using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TestApi;
using Xunit;

public class ApiIntegrationTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetQuery_Valid_Returns200()
    {
        var client = factory.CreateClient();
        var resp = await client.GetAsync("/test/query?Name=ok");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }

    [Fact]
    public async Task GetQuery_Invalid_Returns400_WithErrors()
    {
        var client = factory.CreateClient();
        var resp = await client.GetAsync("/test/query?Name=bad");
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        var errors = await resp.Content.ReadFromJsonAsync<List<string>>();
        Assert.NotNull(errors);
        Assert.Single(errors);
        Assert.Equal("Name cannot be 'bad'", errors[0]);
    }

    [Fact]
    public async Task PostBody_Valid_Returns200()
    {
        var client = factory.CreateClient();
        var body = new FakeBaseRequest
        {
            Name = "ok"
        };

        var resp = await client.PostAsJsonAsync("/test/body", body);
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }

    [Fact]
    public async Task PostBody_Invalid_Returns400_WithErrors()
    {
        var client = factory.CreateClient();
        var body = new FakeBaseRequest
        {
            Name = "bad"
        };

        var resp = await client.PostAsJsonAsync("/test/body", body);
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        var errors = await resp.Content.ReadFromJsonAsync<List<string>>();
        Assert.NotNull(errors);
        Assert.Single(errors);
        Assert.Equal("Name cannot be 'bad'", errors[0]);
    }
}
