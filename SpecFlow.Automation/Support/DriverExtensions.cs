
using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SpecFlow.Automation.Support
{
    public enum ByType
    {
        XPath,
        Id,
        TagName
    }

    public static class DriverExtensions
    {
        public static string GetTitle(this IWebDriver driver)
        {
            return driver.Title;
        }

        private static By GetBy(string locator, ByType by = ByType.XPath)
        {
            switch (by)
            {
                case ByType.XPath:
                    return By.XPath(locator);
                case ByType.Id:
                    return By.Id(locator);
                case ByType.TagName:
                    return By.TagName(locator);
                default:
                    throw new NotSupportedException();
            }
        }

        public static bool WaitUntilUrlContains(this IWebDriver driver, string text, int waitingTime = 100)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(waitingTime))
                .Until(ExpectedConditions.UrlContains(text));
        }

        public static IWebElement WaitUntilExists(this IWebDriver driver, string locator, int waitingTime = 100, ByType byType = ByType.XPath)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(waitingTime)).Until(ExpectedConditions.ElementExists(GetBy(locator, byType)));
        }

        public static IWebElement WaitUntilVisible(this IWebDriver driver, string locator, int waitingTime = 100, ByType byType = ByType.XPath)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(waitingTime)).Until(ExpectedConditions.ElementIsVisible(GetBy(locator, byType)));
        }

        public static IWebElement WaitUntilClickable(this IWebDriver driver, string locator, int waitingTime = 100, ByType byType = ByType.XPath)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(waitingTime)).Until(ExpectedConditions.ElementToBeClickable(GetBy(locator, byType)));
        }

        public static bool WaitUntilHidden(this IWebDriver driver, string locator, int waitingTime = 100, ByType byType = ByType.XPath)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(waitingTime)).Until(ExpectedConditions.InvisibilityOfElementLocated(GetBy(locator, byType)));
        }

        public static IAlert WaitTillAlertPresent(this IWebDriver driver, int waitingTime = 10)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(waitingTime)).Until(ExpectedConditions.AlertIsPresent());
        }

        public static void Click(this IWebDriver driver, string locator, ByType byType = ByType.XPath, bool useJs = false)
        {
            WaitUntilClickable(driver, locator, byType: byType);

            var ele = driver.FindElement(GetBy(locator, byType));
            MoveToElement(driver, ele);

            if (useJs)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", ele);
            }
            else
            {
                try
                {
                    ele.Click();
                }
                catch (ElementClickInterceptedException e)
                {
                    ClickElementIfPresent(driver, "//div[@id='toast-container']/div/div[@class='toast-message']");
                    ele.Click();
                }
            }
        }

        public static void Zoom(this IWebDriver driver, int sizeInPercentage)
        {
            var size = decimal.Divide(sizeInPercentage, 100);
            var t = size.ToString();
            IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
            String zoomJS = string.Format("document.body.style.zoom='{0}'", size.ToString());
            executor.ExecuteScript(zoomJS);
        }

        public static IReadOnlyCollection<IWebElement> GetWebElements(this IWebDriver driver, string locator, ByType byType = ByType.XPath)
        {
            return driver.FindElements(GetBy(locator, byType));
        }

        public static int GetWebElementCount(this IWebDriver driver, string locator, ByType byType = ByType.XPath)
        {
            return driver.FindElements(GetBy(locator, byType)).Count;
        }

        public static string GetText(this IWebDriver driver, string locator, ByType byType = ByType.XPath)
        {
            driver.WaitUntilExists(locator, byType: byType);

            var ele = driver.FindElement(GetBy(locator, byType));
            MoveToElement(driver, ele);

            return ele.Text;
        }

        public static void ClickElementIfPresent(this IWebDriver driver, string locator, ByType byType = ByType.XPath)
        {
            if (Exists(driver, locator, byType))
            {
                Click(driver, locator, byType);
            }
        }

        public static bool Exists(this IWebDriver driver, string locator, ByType byType = ByType.XPath)
        {
            return driver.FindElements(GetBy(locator, byType)).Count > 0;
        }

        public static bool IsDisplayed(this IWebDriver driver, string locator, ByType byType = ByType.XPath)
        {
            return driver.FindElement(GetBy(locator, byType)).Displayed;
        }

        public static bool IsChecked(this IWebDriver driver, string locator, ByType byType = ByType.XPath)
        {
            return driver.FindElement(GetBy(locator, byType)).Selected;
        }

        public static bool IsEnabled(this IWebDriver driver, string locator, ByType byType = ByType.XPath)
        {
            return driver.FindElement(GetBy(locator, byType)).Enabled;
        }

        public static string GetValue(this IWebDriver driver, string locator, ByType byType = ByType.XPath)
        {
            var ele = WaitUntilClickable(driver, locator, byType: byType);
            if (ele != null
                && ele.Displayed)
            {
                MoveToElement(driver, ele);
                return ele.GetAttribute("value");
            }
            return string.Empty;
        }

        public static string GetAttributeValue(this IWebDriver driver, string locator, ByType byType = ByType.XPath, string attribute = "Class")
        {
            var ele = driver.FindElement(GetBy(locator, byType));
            if (ele != null
                && ele.Displayed)
            {
                MoveToElement(driver, ele);
                return ele.GetAttribute(attribute);
            }

            return string.Empty;
        }

        public static void SetText(this IWebDriver driver, string locator, ByType byType = ByType.XPath, string text = "")
        {
            var ele = WaitUntilClickable(driver, locator, byType: byType);
            if (ele != null
                && ele.Displayed)
            {
                MoveToElement(driver, ele);
                ele.Clear();
                ele.SendKeys(text);
            }
        }

        public static void ClearText(this IWebDriver driver, string locator, ByType byType = ByType.XPath)
        {
            var ele = WaitUntilClickable(driver, locator, byType: byType);
            if (ele != null
                && ele.Displayed)
            {
                MoveToElement(driver, ele);
                ele.Clear();
            }
        }

        public static void Select2OptionText(this IWebDriver driver, string locator, ByType byType = ByType.XPath, string text = "")
        {
            Click(driver, locator, byType);
            ApplyActionToEnterText(driver, text);
        }

        public static void ApplyActionToEnterText(this IWebDriver driver, string text)
        {
            Actions actions = new Actions(driver);
            actions.SendKeys(text)
                .SendKeys(Keys.Enter)
                .Build()
                .Perform();
        }

        public static void ApplyActionENTER(this IWebDriver driver)
        {
            Actions actions = new Actions(driver);
            actions.SendKeys(Keys.Enter)
                .Build()
                .Perform();
        }

        public static void ApplyActionTAB(this IWebDriver driver, int times = 1)
        {
            Actions actions = new Actions(driver);
            for (int i = 0; i < times; i++)
            {
                actions.SendKeys(Keys.Tab)
                    .Build()
                    .Perform();
            }
        }

        public static void ApplyActionRightClick(this IWebDriver driver, string locator, ByType byType = ByType.XPath)
        {
            var ele = driver.FindElement(GetBy(locator, byType));
            MoveToElement(driver, ele);
            Actions actions = new Actions(driver);
            actions.ContextClick(ele)
                .Build()
                .Perform();
        }

        public static void ApplyActionDoubleClick(this IWebDriver driver, string locator, ByType byType = ByType.XPath)
        {
            var ele = driver.FindElement(GetBy(locator, byType));
            MoveToElement(driver, ele);
            Actions actions = new Actions(driver);
            actions.DoubleClick(ele)
                .Build()
                .Perform();
        }

        public static void SelectOptionValue(this IWebDriver driver, string locator, ByType byType = ByType.XPath, string value = "")
        {
            var ele = driver.FindElement(GetBy(locator, byType));
            SelectElement oSelect = new SelectElement(ele);
            oSelect.SelectByValue(value);
        }

        public static void SelectOptionText(this IWebDriver driver, string locator, ByType byType = ByType.XPath, string text = "")
        {
            var ele = driver.FindElement(GetBy(locator, byType));
            SelectElement oSelect = new SelectElement(ele);
            oSelect.SelectByText(text);
        }

        public static IWebElement GetSelectedOption(this IWebDriver driver, string locator, ByType byType = ByType.XPath)
        {
            var ele = driver.FindElement(GetBy(locator, byType));
            SelectElement oSelect = new SelectElement(ele);
            return oSelect.SelectedOption;
        }

        public static string GetSelectedOptionText(this IWebDriver driver, string locator, ByType byType = ByType.XPath)
        {
            return GetSelectedOption(driver, locator, byType).Text;
        }

        public static IList<IWebElement> GetSelectedOptions(this IWebDriver driver, string locator, ByType byType = ByType.XPath)
        {
            var ele = driver.FindElement(GetBy(locator, byType));
            SelectElement oSelect = new SelectElement(ele);
            return oSelect.Options;
        }

        public static void SelectOptionByIndex(this IWebDriver driver, string locator, ByType byType = ByType.XPath, int index = -1)
        {
            var ele = driver.FindElement(GetBy(locator, byType));
            SelectElement oSelect = new SelectElement(ele);
            oSelect.SelectByIndex(index);
        }

        public static void MoveToElement(this IWebDriver driver, IWebElement ele)
        {
            IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
            executor.ExecuteScript("arguments[0].scrollIntoView();", ele);
        }

        public static void RefreshPage(this IWebDriver driver)
        {
            driver.Navigate().Refresh();
            WaitUntilExists(driver, "//div[@class='quipt-logo']");
        }
    }
}