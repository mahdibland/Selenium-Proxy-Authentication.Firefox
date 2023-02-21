# Selenium Proxy Authentication For Firefox

> Easily add your auth proxies to your Firefox (Gecko) Driver with one line of code With Extension

## Installation

> You Can Simply Add this Library to your project with Nuget Package: <a href="https://www.nuget.org/packages/SeleniumProxyAuthentication.Firefox/">
    <img src="https://www.nuget.org/Content/gallery/img/logo-header.svg" width="80" height="25"/>
    </a>
```markdown
Install-Package SeleniumProxyAuthentication.Firefox -Version 1.0.0
```
> ! You need to download the "Nightly" or "Developer Edition" of firefox to use this library because in the original browser extensions should be verified before installing since version 43

## How to Use it

- First you need to download the gecko driver from here (make sure that the version of your firefox browser should be match with the version of gecko driver that you download!)

```
https://github.com/mozilla/geckodriver/releases
```
- Then put the geckodriver.exe in your project bin directory

- Create a new Instance of FirefoxOptions and define the FirefoxProfile (if path of profile not set it will generate the profile randomly)

```C#
private static readonly FirefoxOptions FirefoxOptions = new() {Profile = new FirefoxProfile()};
```
 
- Pass the FirefoxOptions Instance to a new Instance of FirefoxDriver

```C#
IWebDriver driver = new FirefoxDriver(FirefoxOptions);
```

- Attach your proxy to the instance of firefox driver using extension method "AddProxyAuthenticationExtension"

```C#
driver.AddProxyAuthenticationExtension(new SeleniumProxyAuthentication.Proxy(
                    ProxyProtocols.HTTP,
                    "host:port:username:password"
                    ));

driver.Navigate().GoToUrl(new Uri("https://github.com/"));
Thread.Sleep(5000);
driver.Quit();
```

- Remove Entire Cache That Created By Extensions using this method

```C#
driver.DeleteExtensionsCache();
```

* also see the sample project to see how it's work <a href="https://github.com/mahdibland/Selenium-Proxy-Authentication.Firefox/blob/74036ffc56d4d05a65355e805cb0dc55243e3a5c/SeleniumProxyAuthentication.Sample/Program.cs">Link to sample project</a>

##  Guides

#### Proxy Format
- Support all kind of proxy types (i tested http/s, socks5)
* Host:Port:Username:Password
* Host:Port

## Licence

[![License](http://img.shields.io/:license-mit-blue.svg?style=flat-square)](https://github.com/mahdibland/Selenium-Proxy-Authentication.Firefox)
