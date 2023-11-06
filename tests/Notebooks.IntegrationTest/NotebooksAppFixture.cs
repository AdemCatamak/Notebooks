using Microsoft.AspNetCore.Mvc.Testing;

namespace Notebooks.IntegrationTest;

[CollectionDefinition(NotebooksAppFixture.NOTEBOOKS_APP_FIXTURE_KEY)]
public class NotebooksAppFixtureDefinition : ICollectionFixture<NotebooksAppFixture>
{
}

public class NotebooksAppFixture : IDisposable
{
    public const string NOTEBOOKS_APP_FIXTURE_KEY = "NotebooksAppFixtureKey";

    private readonly NotebooksInfra _notebooksInfra;
    public readonly WebApplicationFactory<Notebooks.Api.Program> Factory;


    public NotebooksAppFixture()
    {
        _notebooksInfra = new NotebooksInfra();
        Factory = new WebApplicationFactory<Notebooks.Api.Program>();
    }

    public HttpClient GetClient()
    {
        return Factory.CreateClient();
    }

    public void Dispose()
    {
        Factory.Dispose();
        _notebooksInfra.Dispose();
    }
}