using System.Net.Http.Json;

namespace Todo.ApiIntegrationTests.Setup;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected readonly HttpClient Client;
    protected readonly IntegrationTestWebApplicationFactory Factory;

    protected IntegrationTestBase()
    {
        Factory = new IntegrationTestWebApplicationFactory();
        Client = Factory.CreateClient();
    }

    public async ValueTask InitializeAsync()
    {
        await Factory.InitializeAsync();
    }

    public async ValueTask DisposeAsync()
    {
        Client.Dispose();
        await Factory.DisposeAsync();
    }

    protected async Task<T?> GetJsonAsync<T>(string url)
    {
        var response = await Client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    protected async Task<HttpResponseMessage> PostJsonAsync<T>(string url, T data)
    {
        return await Client.PostAsJsonAsync(url, data);
    }

    protected async Task<HttpResponseMessage> PutJsonAsync<T>(string url, T data)
    {
        return await Client.PutAsJsonAsync(url, data);
    }

    protected async Task<HttpResponseMessage> DeleteAsync(string url)
    {
        return await Client.DeleteAsync(url);
    }
}