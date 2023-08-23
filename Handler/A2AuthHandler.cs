using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using A2.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace A2.Handler
{
    public class A2AuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IA2Repo _repository;

        public A2AuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IA2Repo repository)
            : base(options, logger, encoder, clock)
        {
            _repository = repository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                Response.Headers.Add("WWW-Authenticate", "Basic");
                return AuthenticateResult.Fail("Authorization header not found.");
            }

            string userName;
            string password;

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');

                userName = credentials[0];
                password = credentials[1];
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            if (!await _repository.IsUserRegistered(userName, password))
            {
                return AuthenticateResult.Fail("Invalid Username or Password");
            }

            var claims = new[]
            {
                new Claim("RegisteredUser", "true"),
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}