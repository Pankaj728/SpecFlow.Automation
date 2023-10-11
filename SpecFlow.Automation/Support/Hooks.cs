using BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using TechTalk.SpecFlow;
using System;
using System.IO;
using System.Text.Json;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework.Interfaces;
using NUnit.Framework;

namespace SpecFlow.Automation.Support
{
    [Binding]
    public class Hooks
    {
        protected IWebDriver _driver;
        private readonly IObjectContainer _container;
        public Hooks(IObjectContainer container)
        {
            _container = container;
        }

        [BeforeScenario]
        public void InitializeWebDriver()
        {
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Support", "config.json");
            string configJson = File.ReadAllText(configFilePath);
            AppConfigModel config = JsonSerializer.Deserialize<AppConfigModel>(configJson);

            string browserType = config.BrowserType;
            string url = config.Url;

            if (browserType.Equals("chrome", StringComparison.OrdinalIgnoreCase))
            {
                _driver = new ChromeDriver();
            }
            else if (browserType.Equals("firefox", StringComparison.OrdinalIgnoreCase))
            {
                _driver = new FirefoxDriver();
            }
            else
            {
                throw new Exception("Unsupported browser type in config.");
            }

            _container.RegisterInstanceAs<IWebDriver>(_driver);

            _driver.Navigate().GoToUrl(url);

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
            _driver.Manage().Window.Maximize();
        }

        [AfterScenario]
        public void CleanupWebDriver()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var fileName = string.Format("{0}{1}.png", TestContext.CurrentContext.Test.Name, DateTime.Now.ToString("yyyyMMddHHmmss"));
                string directory = AppDomain.CurrentDomain.BaseDirectory;
                var screenShotFolder = Path.Combine(new[] { directory, "ScreenShot" });
                if (!Directory.Exists(screenShotFolder))
                {
                    Directory.CreateDirectory(screenShotFolder);
                }
                var saveTo = string.Format("{0}\\{1}", screenShotFolder, fileName);
                Screenshot ss = ((ITakesScreenshot)_driver).GetScreenshot();
                ss.SaveAsFile(saveTo, ScreenshotImageFormat.Png);
            }
            _driver?.Quit();

        }        
    }
}