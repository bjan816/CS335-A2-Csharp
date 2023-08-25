using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using A2.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace A2.Handler
{
    public static class ClaimTypes
    {
        public const string RegisteredUser = "RegisteredUser";
        public const string IsOrganizer = "IsOrganizer";
    }

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

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
                var username = credentials[0];
                var password = credentials[1];

                if (Request.Path.Value != null)
                {
                    var user = await _repository.IsUserRegistered(username, password);
                    var organizer = await _repository.IsUserOrganizer(username, password);

                    List<Claim> claims = new List<Claim>();

                    if (user)
                    {
                        claims.Add(new Claim(ClaimTypes.RegisteredUser, username));
                    }

                    if (organizer)
                    {
                        claims.Add(new Claim(ClaimTypes.IsOrganizer, username));
                    }

                    if (claims.Count > 0)
                    {
                        var identity = new ClaimsIdentity(claims, Scheme.Name);
                        var principal = new ClaimsPrincipal(identity);
                        var ticket = new AuthenticationTicket(principal, Scheme.Name);

                        return AuthenticateResult.Success(ticket);
                    }
                }

                return AuthenticateResult.Fail("Invalid username or password.");
            }
            catch
            {
                return AuthenticateResult.Fail("Error occurred during authentication.");
            }
        }
    }
}