using Microsoft.AspNetCore.Authentication;

namespace LuTool.Authentication.RequestHeader
{
    public class RequestHeaderOptions : AuthenticationSchemeOptions
    {
        public string Token { get; set; }
    }
}