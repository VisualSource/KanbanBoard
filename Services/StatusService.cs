public class StatusService
{
    private readonly HttpClient http;

    public StatusService(IHttpClientFactory clientFactory)
    {
        http = clientFactory.CreateClient();
    }

    public async Task<DB.Tables.Status[]> GetStatus(string Id)
    {

        var response = await http.GetAsync($"http://localhost:5176/api/columns/{Id}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<DB.Tables.Status[]>() ?? Array.Empty<DB.Tables.Status>();
    }
}