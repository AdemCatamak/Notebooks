using System.Net;
using System.Net.Http.Json;

namespace Notebooks.IntegrationTest.Tests;

[Collection(NotebooksAppFixture.NOTEBOOKS_APP_FIXTURE_KEY)]
public class CreateNotebookTests
{
    private readonly NotebooksAppFixture _appFixture;

    public CreateNotebookTests(NotebooksAppFixture appFixture)
    {
        _appFixture = appFixture;
    }

    [Fact]
    public async Task WhenTextIsSend__ResponseShouldBe201()
    {
        using HttpClient client = _appFixture.GetClient();
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "notebooks");
        httpRequestMessage.Content = JsonContent.Create(new { Title = "title-1", Content = "some text content" });
        using HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage);
        Assert.Equal(HttpStatusCode.Created, httpResponseMessage.StatusCode);
    }
}