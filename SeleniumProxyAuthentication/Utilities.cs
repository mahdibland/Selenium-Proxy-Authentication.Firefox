using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace SeleniumProxyAuthentication
{
    internal class Utilities
    {
        /// <summary>
        /// the characters that not allowed to using in name of files and folders
        /// </summary>
        internal static char[] NotAllowedChars = new[] { '\\', '/', '*', ':', '?', '"', '|', '<', '>' };
        internal static Random rnd = new Random();

        internal static Func<string, string> GetValidName = name =>
            NotAllowedChars.Select(x => (name = name.Replace(x, '!'))).Last();

        internal static Func<string> GetCachePath = () =>
        {
            var cachePath = Path.Combine(GenerateXpi.GetAppDataPath(), "ProxyAuthCache");
            try
            {
                if (!Directory.Exists(cachePath))
                    Directory.CreateDirectory(cachePath);
            }
            catch
            {
                // ignored
            }

            return cachePath;
        };

        internal static Func<string, Proxy, string> GetTempFolder = (path, proxy) =>
        {
            var currentFolder = Path.Combine(path, Utilities.GetValidName(proxy.Host + proxy.Port));
            while (Directory.Exists(currentFolder))
            {
                currentFolder += rnd.Next(0, 10000000);
            }
            //Directory of .xip File
            Directory.CreateDirectory(currentFolder);
            return currentFolder;
        };

        internal static Func<string, string> GetXpiDetailsFolder = path =>
        {
            var detailsFolder = Path.Combine(path, "Details");
            try
            {
                if (Directory.Exists(detailsFolder))
                {
                    Directory.Delete(detailsFolder, true);
                }
            }
            catch
            {
                // ignored
            }

            //Directory of manifest and background files
            Directory.CreateDirectory(detailsFolder);
            return detailsFolder;
        };

        internal static Action<XpiManifest, string, string, Proxy> WriteDetailsFiles =
             (xpiManifest, manifestLocation, backgroundLocation, proxy) =>
            {
                using var streamWriter = new StreamWriter(manifestLocation);
                if (xpiManifest == null)
                {
                    streamWriter.Write(GenerateXpi.GetManifest(new XpiManifest
                    {
                        version = "1.0.1b",
                        manifest_version = 2,
                        name = "Proxy Authentication",
                        permissions = new List<string>
                        {
                            "browsingData",
                            "proxy",
                            "storage",
                            "tabs",
                            "webRequest",
                            "webRequestBlocking",
                            "downloads",
                            "notifications",
                            "<all_urls>"
                        },
                        background = new BackGround { scripts = new List<string> { "background.js" } },
                        browser_specific_settings = new BrowserSpecificSettings{gecko = new Gecko{ id = "myproxy@example.org" } }
                    }));
                }
                else
                {
                    streamWriter.Write(GenerateXpi.GetManifest(new XpiManifest
                    {
                        version = xpiManifest.version,
                        manifest_version = xpiManifest.manifest_version,
                        name = xpiManifest.name,
                        permissions = xpiManifest.permissions,
                        background = xpiManifest.background,
                        browser_specific_settings = xpiManifest.browser_specific_settings
                    }));
                }

                using var streamWriter2 = new StreamWriter(backgroundLocation);
                streamWriter2.Write(GenerateXpi.GenerateXpiCode(proxy));
            };

        internal static Func<string, string, string> CreateExtension = (tempFolder, crxDetails) =>
        {
            var crxLocation = Path.Combine(tempFolder, "AuthFirefoxExtension.xpi");
            try
            {
                if (File.Exists(crxLocation))
                    File.Delete(crxLocation);
            }
            catch
            {
                // ignored
            }

            ZipFile.CreateFromDirectory(crxDetails, crxLocation);
            return crxLocation;
        };
    }
}
