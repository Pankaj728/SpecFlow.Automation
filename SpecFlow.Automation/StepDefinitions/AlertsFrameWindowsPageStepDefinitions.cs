using NUnit.Framework;
using OpenQA.Selenium;
using SpecFlow.Automation.Support;
using TechTalk.SpecFlow;

namespace SpecFlow.Automation.StepDefinitions
{
    [Binding]
    public sealed class AlertsFrameWindowsPageStepDefinitions
    {
        private readonly IWebDriver _driver;


        public AlertsFrameWindowsPageStepDefinitions(IWebDriver driver)
        {
            _driver = driver;
        }

        #region Xpath Repo

        private const string specifcActionButton = "//button[@type='button'][text()='{0}']";
        private const string newWindowMessage = "Knowledge increases by sharing but not by saving. Please share this website with your friends and in your organization.";
        #endregion



        [When(@"Perform and assert ""([^""]*)"" Window action")]
        public void WhenPerformAndAssertWindowAction(string windowAction)
        {
            switch (windowAction.ToLower())
            {
                case "new tab":
                    AssertNewTabWindow(string.Format(specifcActionButton, windowAction));
                    break;
                case "new window":
                    AssertNewTabWindow(string.Format(specifcActionButton, windowAction));
                    break;
                case "new window message":
                    AssertNewTabWindow(string.Format(specifcActionButton, windowAction), newWindowMessage);
                    break;
                default:
                    throw new ArgumentException("Unsupported action: " + windowAction);
            }
        }


        [When(@"Perform and assert ""([^""]*)"" Alert action")]
        public void WhenPerformAndAssertAlertAction(string alertAction)
        {
            switch (alertAction.ToLower())
            {
                case "normal alert":
                    AssertNormalAlert();
                    break;
                case "delayed alert":
                    AssertDelayedAlert();
                    break;
                case "confirm box":
                    AssertConfirmBoxAlert();
                    break;
                case "prompt box":
                    AssertPromptBoxAlert();
                    break;
                default:
                    throw new ArgumentException("Unsupported action: " + alertAction);
            }
        }


















        #region Private Section
        private void AssertNewTabWindow(string button, string message)
        {
            string originalHandle = _driver.CurrentWindowHandle;
            _driver.Click(button);
            var windowHandles = _driver.WindowHandles.ToList();

            string newTabHandle = windowHandles.FirstOrDefault(handle => handle != originalHandle);

            if (newTabHandle != null)
            {

                _driver.SwitchTo().Window(newTabHandle);
                // var t = _driver.GetText("body", ByType.TagName);
                // Assert.AreEqual("message", _driver.GetText("//body",ByType.TagName));

                _driver.Close();
                _driver.SwitchTo().Window(originalHandle);
            }
            else
            {
                Assert.Fail("New tab was not opened.");
            }
        }



        private void AssertNewTabWindow(string button)
        {
            string originalHandle = _driver.CurrentWindowHandle;
            _driver.Click(button);
            var windowHandles = _driver.WindowHandles.ToList();

            string newTabHandle = windowHandles.FirstOrDefault(handle => handle != originalHandle);

            if (newTabHandle != null)
            {
                _driver.SwitchTo().Window(newTabHandle);

                Assert.AreEqual("https://demoqa.com/sample", _driver.Url);

                _driver.Close();
                _driver.SwitchTo().Window(originalHandle);
            }
            else
            {
                Assert.Fail("New tab was not opened.");
            }
        }

        private void AssertPromptBoxAlert()
        {
            _driver.Click("//button[@id='promtButton']");
            Assert.AreEqual("Please enter your name", GetAlertText());
            ConfirmAlert();
        }

        private void AssertConfirmBoxAlert()
        {
            _driver.Click("//button[@id='confirmButton']");
            Assert.AreEqual("Do you confirm action?", GetAlertText());
            ConfirmAlert();
        }
        private void AssertNormalAlert()
        {
            _driver.Click("//button[@id='alertButton']");
            Assert.AreEqual("You clicked a button", GetAlertText());
            ConfirmAlert();
        }

        private void AssertDelayedAlert()
        {
            _driver.Click("//button[@id='timerAlertButton']");
            Assert.AreEqual("This alert appeared after 5 seconds", GetAlertText());
            ConfirmAlert();
        }

        private void ConfirmAlert()
        {
            _driver.WaitTillAlertPresent();
            IAlert alert = _driver.SwitchTo().Alert();
            alert.Accept();
        }

        private string GetAlertText()
        {
            _driver.WaitTillAlertPresent();
            IAlert alert = _driver.SwitchTo().Alert();
            return alert.Text;
        }

        #endregion
    }
}
