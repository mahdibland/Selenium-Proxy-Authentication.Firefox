using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace SeleniumProxyAuthentication.Sample
{
    public record Program
    {
        private static readonly FirefoxOptions FirefoxOptions = new() {Profile = new FirefoxProfile()};
        public static void Main()
        {
            IWebDriver driver = new FirefoxDriver(FirefoxOptions);
            try
            {
                //with proxy credential
                driver.AddProxyAuthenticationExtension(new Proxy(ProxyProtocols.HTTP, "209.127.191.180:9279:iazgeluc:buxw7bzlke0x"));

                ////without proxy credential
                //(driver as FirefoxDriver)?.AddProxyAuthenticationExtension(new Proxy(ProxyProtocols.HTTP, "proxy_server:proxy_port"));

                driver.Navigate().GoToUrl(new Uri("https://github.com/"));
                Thread.Sleep(5000);
                driver.Quit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                driver.DeleteExtensionsCache();
            }
        }
    }
}