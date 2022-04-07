using System;
using System.Text.Json;

namespace SeleniumProxyAuthentication
{
    internal static class GenerateXpi
    {
        /// <summary>
        /// Convert Protocol to Scheme
        /// </summary>
        internal static readonly Func<ProxyProtocols, string> GetScheme = protocol =>
            (protocol == ProxyProtocols.HTTP) ? "http" :
            (protocol == ProxyProtocols.HTTPS) ? "https" :
            (protocol == ProxyProtocols.SOCKS4) ? "socks4" :
            (protocol == ProxyProtocols.SOCKS5) ? "socks5" : String.Empty;

        /// <summary>
        /// Get Proxy Rule For Protocols (Always use singleProxy Not proxyForHttp for any http, https, ftp server)
        /// </summary>
        //internal static readonly Func<Proxy, string> GetProxyRule = (proxy) => proxy.ProxyProtocol == ProxyProtocols.HTTP || proxy.ProxyProtocol == ProxyProtocols.HTTPS ? "singleProxy" : "fallbackProxy";
        internal static readonly Func<Proxy, string> GetProxyRule = (proxy) => "singleProxy";

        /// <summary>
        /// Get APPData Path from Machine
        /// </summary>
        internal static Func<string> GetAppDataPath =
            () => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        /// <summary>
        /// Generate Manifest Json
        /// </summary>
        internal static Func<XpiManifest, string> GetManifest = xpiManifest => JsonSerializer.Serialize(xpiManifest);

        /// <summary>
        /// Generate Js Code For Xpi Background
        /// </summary>
        internal static Func<Proxy, string> GenerateXpiCode = proxy =>
            $@"
var config2 = {{
    mode: ""fixed_servers"",
    rules: {{
        " + GetProxyRule(proxy) + @": {
            scheme: '" + GetScheme(proxy.ProxyProtocol) + @"',
            host: '" + proxy.Host + @"',
            port: " + proxy.Port + @"
        },
        bypassList: []
    }
}

function proxyRequest(request_data)
{
    return {
        type: '" + GetScheme(proxy.ProxyProtocol) + @"',
        host: '" + proxy.Host + @"', 
        port: " + proxy.Port + @"
    };
}
browser.proxy.settings.set({ value: config2, scope: ""regular""}, function() { });
function callbackFn(details) {return {authCredentials: {username: '" + proxy.Credential.UserName + @"',password: '" + proxy.Credential.Password + @"'}}};
browser.webRequest.onAuthRequired.addListener(
    callbackFn,
    { urls: [""<all_urls>""]},
    ['blocking']
);
browser.proxy.onRequest.addListener(proxyRequest, { urls: [""<all_urls>""]});";
    }
}