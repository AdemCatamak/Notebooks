using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notebooks.Domain.Exceptions;

namespace Notebooks.IntegrationTest.Tests;

[Collection(NotebooksAppFixture.NOTEBOOKS_APP_FIXTURE_KEY)]
public class GetNotebookTests
{
    private readonly NotebooksAppFixture _appFixture;

    public GetNotebookTests(NotebooksAppFixture appFixture)
    {
        _appFixture = appFixture;
    }

    [Fact]
    public async Task WhenNotebookNotExist__ResponseShouldBe404()
    {
        using HttpClient client = _appFixture.GetClient();
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"notebooks/{Guid.NewGuid()}");
        using HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage);
        Assert.Equal(HttpStatusCode.NotFound, httpResponseMessage.StatusCode);
    }

    [Fact]
    public async Task WhenNotebookExist__ResponseShouldBe200()
    {
        const string title = "get-test-1";
        const string content = "get-test-1-content";

        var id = await CreateNotebookAsync(title, content);
        using HttpClient client = _appFixture.GetClient();
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"notebooks/{id}");
        using HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage);
        Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);

        var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<JObject>(responseContent);
        Assert.NotNull(response);
        Assert.Equal(title, response!.Property("title")?.Value.ToString());
        Assert.Equal(content, response!.Property("content")?.Value.ToString());
    }

    private async Task<string> CreateNotebookAsync(string title, string content)
    {
        using HttpClient client = _appFixture.GetClient();
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "notebooks");
        httpRequestMessage.Content = JsonContent.Create(new { Title = title, Content = content });
        using HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage);

        var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<JObject>(responseContent);
        var id = response!.Property("id")?.Value.ToString() ?? throw new DevelopmentException("Id is not found in response");
        return id;
    }
}