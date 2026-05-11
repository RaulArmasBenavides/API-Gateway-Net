using System.Net;

namespace Gateway.Api.Handlers;

public class BlacklistHandler : DelegatingHandler
{
    private readonly IConfiguration _config;
    private static HashSet<string> _blacklistedIps = new();
    private static HashSet<string> _blacklistedClients = new();

    public BlacklistHandler(IConfiguration config)
    {
        _config = config;
        LoadBlacklist();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var clientIp = request.Content?.Headers.ContentLength?.ToString() ?? "unknown";
        var clientId = request.Headers.FirstOrDefault(h => h.Key == "X-ClientId").Value?.FirstOrDefault();

        if (_blacklistedIps.Contains(clientIp) || _blacklistedClients.Contains(clientId ?? ""))
        {
            return new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                Content = new StringContent("Acceso denegado: Cliente en lista negra")
            };
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private void LoadBlacklist()
    {
        var config = _config.GetSection("Blacklist");
        var ips = config.GetSection("IPs").Get<List<string>>() ?? new();
        var clients = config.GetSection("Clients").Get<List<string>>() ?? new();

        _blacklistedIps = new HashSet<string>(ips);
        _blacklistedClients = new HashSet<string>(clients);
    }
}
