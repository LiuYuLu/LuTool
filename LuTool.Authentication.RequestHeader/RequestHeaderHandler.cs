using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LuTool.Authentication.RequestHeader
{
    public class RequestHeaderHandler : AuthenticationHandler<RequestHeaderOptions>
    {
        public RequestHeaderHandler(IOptionsMonitor<RequestHeaderOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
             : base(options, logger, encoder, clock)
        { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(Options.Token))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            var token = Request.Headers[Options.Token].ToString();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, token),
                new Claim(ClaimTypes.Sid, token),
            };

            //根据证件单元创建身份证
            ClaimsIdentity identity = new ClaimsIdentity(claims, Scheme.Name);

            //创建身份证这个证件的携带者：我们叫这个证件携带者为“证件当事人”
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}