using NUnit.Framework;
using OpenQA.Selenium;
using SpecFlow.Automation.Support;
using System;
using TechTalk.SpecFlow;

namespace SpecFlow.Automation.StepDefinitions
{
    [Binding]
    public sealed class MainLandingPageStepDefinitions
    {
        private readonly IWebDriver _driver;

        #region Xpath Repo
        private const string menuSection = "//div[@class='category-cards']";
        private const string specificMenuOption = "//div[@class='category-cards']//div[@class='card-body']/h5[text()='{0}']";
        private const string specificLeftPanelMenuOption = "//div[contains(@class,'left-pannel')]//div[@class='header-text'][text()='{0}']";

        
        #endregion
        public MainLandingPageStepDefinitions(IWebDriver driver)
        {
            _driver = driver;
        }

        [Given(@"User is on Main Page")]
        public void GivenUserIsOnMainPage()
        {
            Assert.IsTrue(_driver.Exists(menuSection));
        }

        [Given(@"Click on ""([^""]*)"" Sections")]
        public void GivenClickOnSections(string menuOption)
        {
            string expectedPage = GetlandingPage(menuOption);
            _driver.Click(string.Format(specificMenuOption, menuOption));
            _driver.WaitUntilUrlContains(expectedPage);
            _driver.WaitUntilClickable(string.Format(specificLeftPanelMenuOption, menuOption));
        }

        private string GetlandingPage(string menuOption)
        {
            //menuOption= menuOption.ToLower();
            switch (menuOption)
            {
                case "Elements":
                    return "elements";
                case "Forms":
                    return "forms";
                case "Alerts, Frame & Windows":
                    return "alertsWindows";
                case "Widgets":
                    return "widgets";
                case "Interactions":
                    return "interaction";
                case "Book Store Application":
                    return "books";
                default:
                    return "Invalid option. Please choose a valid option.";
            }
        }
    }
}
