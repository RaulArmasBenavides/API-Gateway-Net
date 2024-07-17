using System.Net;

namespace Gateway.Api.handlers
{
    public class BlackListHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var myHeader = request.Headers.FirstOrDefault(c => c.Key == "Header");

            if (myHeader.Value != null && myHeader.Value.Any())
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var response = new HttpResponseMessage(HttpStatusCode.BadGateway);
            response.ReasonPhrase = "Your header is not valid";
            return await Task.FromResult<HttpResponseMessage>(response);

        }
    }
}
