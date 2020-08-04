using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Elwark.Account.Web.Services
{
    public class AuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly IAccessTokenProvider _tokenProvider;

        public AuthorizationDelegatingHandler(IAccessTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            var tokenResult = await _tokenProvider.RequestAccessToken();

            if (tokenResult.TryGetToken(out var accessToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Value);

            return await base.SendAsync(request, ct);
        }
    }
}