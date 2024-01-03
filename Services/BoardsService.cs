public class BoardService
{
    private readonly string Root = "http://localhost:5176/api/boards";
    private readonly HttpClient http;

    public BoardService(IHttpClientFactory clientFactory)
    {
        http = clientFactory.CreateClient();
    }

    public async Task<DB.Tables.Board[]> GetBoards()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{Root}");

        var response = await http.SendAsync(request);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<DB.Tables.Board[]>() ?? Array.Empty<DB.Tables.Board>();
    }

    public async Task<DB.Tables.Board?> GetBoard(Guid id)
    {
        var response = await http.GetAsync($"{Root}/{id}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<DB.Tables.Board>();
        return result;
    }

    public async Task<DB.Tables.Board> PostBoard(DB.Tables.Board board)
    {
        var response = await http.PostAsJsonAsync($"{Root}", board);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<DB.Tables.Board>();

        if (result is null) throw new Exception("Failed to create board");

        return result;
    }

    public async Task<DB.Tables.Board> PatchBoard(DB.Tables.Board board)
    {
        var response = await http.PatchAsJsonAsync($"{Root}", board);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<DB.Tables.Board>();

        if (result is null) throw new Exception("Failed to updated content.");

        return result;
    }

    public async Task DeleteBoard(Guid id)
    {
        var response = await http.DeleteAsync($"{Root}?id={id}");
        response.EnsureSuccessStatusCode();
    }
}