using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace SpecFlow.Automation.StepDefinitions
{
    [Binding]
    public sealed class InteractionsStepDefinitions
    {
        private readonly IWebDriver _driver;


        public InteractionsStepDefinitions(IWebDriver driver)
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
        #endregion





    }
}
