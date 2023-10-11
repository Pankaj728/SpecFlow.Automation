using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SpecFlow.Automation.Support;
using System.Globalization;
using TechTalk.SpecFlow;

namespace SpecFlow.Automation.StepDefinitions
{
    [Binding]
    public sealed class WidgetsPageStepDefinitions
    {
        private readonly IWebDriver _driver;


        public WidgetsPageStepDefinitions(IWebDriver driver)
        {
            _driver = driver;
        }

        #region Xpath Repo

        private const string multipleColorTextBox = "//input[@id='autoCompleteMultipleInput']";
        private const string singleColorTextBox = "//input[@id='autoCompleteSingleInput']";
        private const string selectedMultiColor = "//div[@id='autoCompleteMultiple']//div[contains(@class,'auto-complete__value-container')]/div[contains(@class,'-multiValue')]/div[contains(@class,'_label')]";
        private const string selectedSingleColor = "//div[@id='autoCompleteSingle']//div[contains(@class,'auto-complete__value-container')]/div[contains(@class,'single-value')]";
        private const string dateTimeSelectMonthYearLabel = "//div[@class='react-datepicker__current-month react-datepicker__current-month--hasYearDropdown react-datepicker__current-month--hasMonthDropdown'][contains(text(),'{0}')]";
        private const string dateTextBox = "//input[@id='datePickerMonthYearInput']";
        
        //div[@class='react-datepicker__current-month react-datepicker__current-month--hasYearDropdown react-datepicker__current-month--hasMonthDropdown'][contains(text(),'')]
        #endregion




        [When(@"User should be able to select multipleColor")]
        public void WhenUserShouldBeAbleToSelectMultipleColor()
        {
            List<string> colors = new List<string>() { "White", "Red" };

            foreach (var item in colors)
            {
                Thread.Sleep(200);
                _driver.SetText(multipleColorTextBox, text: item);
                Thread.Sleep(100);
                _driver.ApplyActionTAB();
                Assert.IsTrue(_driver.GetWebElements(selectedMultiColor).Any(m => m.Text == item));
            }

        }

        [Then(@"User should be able to select single color")]
        public void ThenUserShouldBeAbleToSelectSingleColor()
        {
            var color = "Red";
            _driver.SetText(singleColorTextBox, text: "Red");
            Thread.Sleep(100);
            _driver.ApplyActionTAB();
            Assert.IsTrue(_driver.GetText(selectedSingleColor) == color);
        }



        [When(@"User should be able to select ""([^""]*)""")]
        public void WhenUserShouldBeAbleToSelect(string inputDate)
        {
            DateTime date = DateTime.ParseExact(inputDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string expectedDate = DateTime.ParseExact(inputDate, "MM/dd/yyyy",null).ToString("MM/dd/yyyy");
            

            string day = date.Day.ToString();
            string daySuffix = GetDaySuffix(date.Day);

            string formattedDate = $"//div[@aria-label='Choose {date:dddd}, {date:MMMM} {day}{daySuffix}, {date:yyyy}']";

            string month = date.ToString("MMMM", CultureInfo.InvariantCulture);
            int year = date.Year;

            _driver.Click("//input[@id='datePickerMonthYearInput']");

            _driver.SelectOptionText("//select[@class='react-datepicker__month-select']", text: month);
            _driver.WaitUntilExists(string.Format(dateTimeSelectMonthYearLabel, month));

            _driver.SelectOptionText("//select[@class='react-datepicker__year-select']", text: year.ToString());
            _driver.WaitUntilExists(string.Format(dateTimeSelectMonthYearLabel, year.ToString()));

            _driver.Click(formattedDate);
            Assert.AreEqual(expectedDate, _driver.GetAttributeValue(dateTextBox, attribute: "value"));
        }


        [When(@"Verify ToolTop functionality")]
        public void WhenVerifyToolTopFunctionality()
        {
            IWebElement elementToHover = _driver.FindElement(By.XPath("//button[@id='toolTipButton']"));
            Actions hover = new Actions(_driver);

            hover.MoveToElement(elementToHover);
            Thread.Sleep(500);
            hover.Perform();
            Assert.IsTrue(_driver.Exists("//button[@id='toolTipButton'][@aria-describedby]"));          
        }








        #region Private Section

        static string GetDaySuffix(int day)
        {
            if (day >= 11 && day <= 13)
            {
                return "th";
            }

            switch (day % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }
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

        #endregion
    }
}
