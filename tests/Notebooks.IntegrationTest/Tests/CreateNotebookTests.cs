using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace Notebooks.IntegrationTest.Tests;

[Collection(NotebooksAppFixture.NOTEBOOKS_APP_FIXTURE_KEY)]
public class CreateNotebookTests
{
    private readonly NotebooksAppFixture _appFixture;
    private readonly ITestOutputHelper _outputHelper;

    public CreateNotebookTests(NotebooksAppFixture appFixture, ITestOutputHelper outputHelper)
    {
        _appFixture = appFixture;
        _outputHelper = outputHelper;
    }

    [Fact]
    public async Task WhenTextIsSend__ResponseShouldBe201()
    {
        using HttpClient client = _appFixture.GetClient();
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "notebooks");
        httpRequestMessage.Content = JsonContent.Create(new { Title = "title-1", Content = "some text content" });
        HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage);
        Assert.Equal(HttpStatusCode.Created, httpResponseMessage.StatusCode);
    }
}