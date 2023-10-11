using NUnit.Framework;
using OpenQA.Selenium;
using SpecFlow.Automation.Support;
using TechTalk.SpecFlow;

namespace SpecFlow.Automation.StepDefinitions
{
    [Binding]
    public sealed class ElementsPageStepDefinitions
    {
        private readonly IWebDriver _driver;

        public ElementsPageStepDefinitions(IWebDriver driver)
        {
            _driver = driver;
        }

        #region Xpath Repo
        private const string textBoxInputField = "//div[@class='text-field-container']//label[text()='{0}']/../following-sibling::div/input";
        private const string textBoxTextAreaField = "//div[@class='text-field-container']//label[text()='{0}']/../following-sibling::div/textarea";
        private const string openElementsSection = "//div[contains(@class,'left-pannel')]//div[@class='header-text'][text()='Elements']/../../following-sibling::div[contains(@class,'show')]";
        private const string specificSubMenu = "//ul[contains(@class,'menu-list')]//span[normalize-space()='{0}']";
        private const string specificSubMenuActive = "//ul[contains(@class,'menu-list')]//span[normalize-space()='{0}']/parent::li[contains(@class,'active')]";
        private const string formSubmit = "//button[@id='submit']";
        private const string textBoxSubmittedSection = "//div[@id='output']//p";
        private const string specificTextBoxSubmittedAttribute = "//div[@id='output']//p[contains(text(),'{0}')]";
        private const string emailIdTextBoxError = "//input[@id='userEmail'][contains(@class,'field-error')]";
        private const string specificEditIconFromWebTable = "//div[contains(@class,'rt-tbody')]//div[@class='rt-tr-group']/div/div[text()='{0}']/following-sibling::div[text()='{1}']/following-sibling::div[text()='{2}']/..//span[contains(@id,'edit-record')]";
        private const string specificDeleteIconFromWebTable = "//div[contains(@class,'rt-tbody')]//div[@class='rt-tr-group']/div/div[text()='{0}']/following-sibling::div[text()='{1}']/following-sibling::div[text()='{2}']/..//span[contains(@id,'delete-record')]";
        private const string firstNameTextBox = "//input[@id='firstName']";
        private const string specificRegistrationFormTextBox = "//div[@class='modal-body']//label[text()='{0}']/../following-sibling::div/input";
        private const string addRecordButton = "//button[@id='addNewRecordButton']";
        private const string specifcActionButton = "//button[@type='button'][text()='{0}']";
        private const string doubleClickMessage = "//p[contains(@id,'ClickMessage')][text()='You have done a double click']";
        private const string rightClickMessage = "//p[contains(@id,'ClickMessage')][text()='You have done a right click']";
        private const string clickMessage = "//p[contains(@id,'ClickMessage')][text()='You have done a dynamic click']";
        private const string specificCheckBox = "//span[contains(text(),'{0}')]/..//span[@class='rct-checkbox']";

        #endregion

        private Dictionary<string, string> textBoxData;
        private Dictionary<string, string> registrationForm;


        [Then(@"User Should navigate to Elements Page")]
        public void ThenUserShouldNavigateToElementsPage()
        {
            Assert.IsTrue(_driver.Exists(openElementsSection));
        }

        [Given(@"Click on ""([^""]*)"" option")]
        public void GivenClickOnOption(string option)
        {
            Assert.Fail();
            _driver.Click(string.Format(specificSubMenu, option));
            _driver.WaitUntilExists(string.Format(specificSubMenuActive, option));
        }


        [When(@"User Fills Details")]
        public void WhenUserFillsDetails(Table table)
        {
            textBoxData = new Dictionary<string, string>();

            foreach (var row in table.Rows)
            {
                string key = row["Key"];
                string value = row["Value"];
                SetText(key, value);
                textBoxData[key] = value;
            }
            _driver.Click(formSubmit);
            _driver.WaitUntilExists(textBoxSubmittedSection);
        }



        [Then(@"User Should be able to verify filled details")]
        public void ThenUserShouldBeAbleToVerifyFilledDetails()
        {
            Assert.AreEqual(textBoxData.Count, _driver.GetWebElementCount(textBoxSubmittedSection));
            foreach (string key in textBoxData.Keys)
            {
                string targetKey;

                if (key == "Full Name")
                {
                    targetKey = "Name";
                }
                else if (key == "Permanent Address")
                {
                    targetKey = "Permananet Address";
                }
                else
                {
                    targetKey = key;
                }
                Assert.IsTrue(_driver.GetText(string.Format(specificTextBoxSubmittedAttribute, targetKey)).Contains(textBoxData[key]));
            }
        }

        [When(@"User Fills InCorrect Details")]
        public void WhenUserFillsInCorrectDetails(Table table)
        {
            foreach (var row in table.Rows)
            {
                string key = row["Key"];
                string value = row["Value"];
                SetText(key, value);
            }
            _driver.Click(formSubmit);
            Assert.IsFalse(_driver.Exists(textBoxSubmittedSection));
        }

        [Then(@"User Should be able to see Error")]
        public void ThenUserShouldBeAbleToSeeError()
        {
            Assert.IsTrue(_driver.Exists(emailIdTextBoxError));
        }

        [When(@"User clicks on edit icon of specific record with First Name ""(.*)"", Last Name ""(.*)"", and Email ""(.*)""")]
        public void WhenUserClicksOnEditIconOfSpecificRecord(string firstName, string lastName, string email)
        {
            _driver.Click(string.Format(specificEditIconFromWebTable, firstName, lastName, email));
            _driver.WaitUntilClickable(firstNameTextBox);
        }

        [Then(@"User AddUpdates record and submit")]
        public void ThenUserAddUpdatesRecordAndSubmit(Table table)
        {
            registrationForm = new Dictionary<string, string>();
            foreach (var row in table.Rows)
            {
                string key = row["Key"];
                string value = row["Value"];
                SetRegistrationText(key, value);
                registrationForm[key] = value;
            }
            _driver.Click(formSubmit);
            _driver.WaitUntilHidden(formSubmit);
        }


        [Then(@"User should see last saved values")]
        public void ThenUserShouldSeeLastSavedValues()
        {
            foreach (string key in registrationForm.Keys)
            {
                Assert.AreEqual(registrationForm[key], _driver.GetAttributeValue(string.Format(specificRegistrationFormTextBox, key),attribute: "value"));
            }

        }

        [When(@"user clicks on Add Button")]
        public void WhenUserClicksOnAddButton()
        {
            _driver.Click(addRecordButton);
            _driver.WaitUntilExists(firstNameTextBox);
        }


        [When(@"User clicks on delete icon of specific record with First Name ""([^""]*)"", Last Name ""([^""]*)"", and Email ""([^""]*)""")]
        public void WhenUserClicksOnDeleteIconOfSpecificRecordWithFirstNameLastNameAndEmail(string firstName, string lastName, string email)
        {
            _driver.Click(string.Format(specificDeleteIconFromWebTable, firstName, lastName, email));
            _driver.WaitUntilHidden(string.Format(specificDeleteIconFromWebTable, firstName, lastName, email));
        }

        [When(@"Perform and assert ""([^""]*)"" action")]
        public void WhenPerformAndAssertAction(string action)
        {
            switch (action.ToLower())
            {
                case "click me":
                    _driver.Click(string.Format(specifcActionButton, action));
                    _driver.WaitUntilExists(clickMessage);
                    break;
                case "double click me":
                    _driver.ApplyActionDoubleClick(string.Format(specifcActionButton, action));
                    _driver.WaitUntilExists(doubleClickMessage);
                    break;
                case "right click me":
                    _driver.ApplyActionRightClick(string.Format(specifcActionButton,action));
                    _driver.WaitUntilExists(rightClickMessage);
                    break;
                default:
                    throw new ArgumentException("Unsupported action: " + action);
            }
        }


        [When(@"Select ""([^""]*)"" Check Box")]
        public void WhenSelectCheckBox(string checkBoxName)
        {
            _driver.Click(string.Format(specificCheckBox, checkBoxName));
            _driver.Click(string.Format("//span[contains(text(),'{0}')]/..//span[@class='rct-checkbox']/../..//button", checkBoxName));
        }

        private const string firstLevelChildCheckBox = "//span[contains(text(),'Home')]/../../following-sibling::ol/li//button";        

        [Then(@"User Should be able to see all child checkbox Selected")]
        public void ThenUserShouldBeAbleToSeeAllChildCheckboxSelected()
        {
            var parentOptionCount = _driver.GetWebElementCount(firstLevelChildCheckBox);
            for (int i = 1; i <= parentOptionCount; i++)
            {
               Assert.IsTrue(_driver.GetAttributeValue($"//span[contains(text(),'Home')]/../../following-sibling::ol/li[{i}]//span[@class='rct-checkbox']").Contains("-check"));
                _driver.Click($"//span[contains(text(),'Home')]/../../following-sibling::ol/li[{i}]//button");
                _driver.WaitUntilClickable($"//span[contains(text(),'Home')]/../../following-sibling::ol/li[{i}]/ol//span[@class='rct-checkbox']");
                Assert.IsTrue(_driver.GetAttributeValue($"//span[contains(text(),'Home')]/../../following-sibling::ol/li[{i}]/ol//span[@class='rct-checkbox']").Contains("-check"));
            }

        }






















        #region Private Section

        private void SetText(string key, string value)
        {
            var inputCount = _driver.GetWebElementCount(string.Format(textBoxInputField, key));
            var textAreaCount = _driver.GetWebElementCount(string.Format(textBoxTextAreaField, key));

            if (inputCount == 1)
            {
                _driver.SetText(string.Format(textBoxInputField, key), text: value);
            }
            else if (textAreaCount == 1)
            {
                _driver.SetText(string.Format(textBoxTextAreaField, key), text: value);
            }
        }

        private void SetRegistrationText(string key, string value)
        {
            _driver.SetText(string.Format(specificRegistrationFormTextBox, key), text: value);
        }

        #endregion
    }
}
