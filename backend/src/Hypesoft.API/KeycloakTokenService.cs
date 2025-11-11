using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class KeycloakTokenService
{
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;

    public KeycloakTokenService(IConfiguration config)
    {
        _config = config;
        _httpClient = new HttpClient();
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var keycloak = _config.GetSection("Keycloak");
        var data = new Dictionary<string, string>
        {
            { "client_id", keycloak["ClientId"]! },
            { "client_secret", keycloak["ClientSecret"]! },
            { "grant_type", "client_credentials" }
        };

        var content = new FormUrlEncodedContent(data);
        var url = $"{keycloak["AuthServerUrl"]}/realms/{keycloak["Realm"]}/protocol/openid-connect/token";

        var response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonDocument.Parse(json);
        return result.RootElement.GetProperty("access_token").GetString()!;
    }
}