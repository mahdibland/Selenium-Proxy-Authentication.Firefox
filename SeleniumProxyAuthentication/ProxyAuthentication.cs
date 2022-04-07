using System;
using System.IO;
using System.Runtime.InteropServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace SeleniumProxyAuthentication
{
    /// <summary>
    /// Auth or Non-Auth Proxy For Firefox
    /// </summary>
    public static class ProxyAuthentication
    {
        /// <summary>
        /// <b>Add Proxy With Extension To Firefox Driver</b>
        /// </summary>
        /// <remarks><b>HTTP/S and Socks5 are tested and working </b> (Didn't Test It With Socks4 Auth Proxies but they should work as well)<br/><br/><b>* You must using the dev or nightly version of firefox</b></remarks>
        /// <param name="browserDriver">use only Firefox Driver</param>
        /// <param name="proxy">Your Proxy With This Format host:port or host:port:user:pass as string</param>
        /// <param name="xpiManifest">Edit Firefox Extension Manifest File (Leave it Empty If You Don't Want To Change It)</param>
        public static void AddProxyAuthenticationExtension<T>(this T browserDriver, Proxy proxy, [Optional] XpiManifest xpiManifest)
        {
            var cachePath = Utilities.GetCachePath();

            var tempFolder = Utilities.GetTempFolder(cachePath, proxy);

            var crxDetailsFolder = Utilities.GetXpiDetailsFolder(tempFolder);

            var manifestLocation = Path.Combine(crxDetailsFolder, "manifest.json");
            var backgroundLocation = Path.Combine(crxDetailsFolder, "background.js");

            Utilities.WriteDetailsFiles(xpiManifest, manifestLocation, backgroundLocation, proxy);

            if (browserDriver is FirefoxDriver firefoxDriver)
            {
                firefoxDriver.InstallAddOnFromFile(Utilities.CreateExtension(tempFolder, crxDetailsFolder));
            }
        }
        /// <summary>
        /// <b>Delete All Files That Made By Extensions</b>
        /// </summary>
        /// <param name="firefoxDriver"></param>
        public static void DeleteExtensionsCache(this IWebDriver firefoxDriver)
        {
            var cacheFolder = Path.Combine(GenerateXpi.GetAppDataPath(), "ProxyAuthCache");
            if(Directory.Exists(cacheFolder))
                Directory.Delete(cacheFolder);
        }
    }
}
