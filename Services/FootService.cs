namespace footApi.Services;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

public class FootService
{
    private readonly HttpClient _httpClient;

    public FootService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        
    }

    public async Task<List<Match>> GetTodayMatchesAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<ApiResponse>("fixtures?date=2024-02-24");
        return response?.Response ?? new List<Match>();
    }

}

public class ApiResponse
{
    public List<Match> Response { get; set; }

}

public class Match
{
    public Fixture Fixture { get; set; }
    public League League { get; set; }
    public Teams Teams { get; set; }
    public Goals Goals { get; set; }
}

public class Fixture {public int Id { get; set; }public string Date { get; set; }}
public class League {public string Name { get; set; } public string Country { get; set; }}
public class Teams {public TeamInfo Home { get; set; } public TeamInfo Away { get; set; }}
public class TeamInfo {public string Name { get; set; }public string Logo { get; set; }}
public class Goals {public int? Home { get; set; }public int? Away{ get; set; }}
