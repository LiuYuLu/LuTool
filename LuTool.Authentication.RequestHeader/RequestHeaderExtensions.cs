using System;
using Microsoft.AspNetCore.Authentication;

namespace LuTool.Authentication.RequestHeader
{
    public static class RequestHeaderExtensions
    {
        public static AuthenticationBuilder AddRequestHeader(this AuthenticationBuilder builder)
            => builder.AddRequestHeader(RequestHeaderDefaults.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddRequestHeader(this AuthenticationBuilder builder, Action<RequestHeaderOptions> configureOptions)
            => builder.AddRequestHeader(RequestHeaderDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddRequestHeader(this AuthenticationBuilder builder, string authenticationScheme, Action<RequestHeaderOptions> configureOptions)
            => builder.AddRequestHeader(authenticationScheme, displayName: null, configureOptions: configureOptions);

        public static AuthenticationBuilder AddRequestHeader(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<RequestHeaderOptions> configureOptions)
        {
            //builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<RequestHeaderOptions>, RequestHeaderPostConfigureOptions>());
            return builder.AddScheme<RequestHeaderOptions, RequestHeaderHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}