Feature: AssertElementPage
Assert different options available in elements page


@mytag
Scenario: Elements_TextBox_NavigateToElements_ShouldSucceed
	Given User is on Main Page
	And Click on "Elements" Sections
	Then User Should navigate to Elements Page



Scenario: Elements_TextBox_AddValidData_ShouldSucceed
	Given Click on "Elements" Sections
	And Click on "Text Box" option
	When User Fills Details
		| Key               | Value                  |
		| Full Name         | Test Name              |
		| Email             | Test@test.com          |
		| Current Address   | Current Address Test   |
		| Permanent Address | Permanent Address Test |
	Then User Should be able to verify filled details


Scenario: Elements_TextBox_AddInValidEmailId_ShouldFail
	Given Click on "Elements" Sections
	And Click on "Text Box" option
	When User Fills InCorrect Details
		| Key   | Value      |
		| Email | Test@test. |
	Then User Should be able to see Error

Scenario: Elements_CheckBox_AssertParentCheckBoxFunctionality_ShouldSucceed
	Given Click on "Elements" Sections
	And Click on "Check Box" option
	When Select "Home" Check Box
	Then User Should be able to see all child checkbox Selected

Scenario: Elements_WebTables_AssertEditFunctionality_ShouldSucceed
	Given Click on "Elements" Sections
	And Click on "Web Tables" option
	When User clicks on edit icon of specific record with First Name "Kierra", Last Name "Gentry", and Email "kierra@example.com"
	Then User AddUpdates record and submit
		| Key        | Value                |
		| First Name | EditTestFirst        |
		| Last Name  | EditTestLast         |
		| Email      | EditTest@example.com |
		| Age        | 40                   |
		| Salary     | 1234                 |
		| Department | Legal                |
	When User clicks on edit icon of specific record with First Name "EditTestFirst", Last Name "EditTestLast", and Email "EditTest@example.com"
	Then User should see last saved values


Scenario: Elements_WebTables_AssertAddFunctionality_ShouldSucceed
	Given Click on "Elements" Sections
	And Click on "Web Tables" option
	When user clicks on Add Button
	Then User AddUpdates record and submit
		| Key        | Value               |
		| First Name | AddTestFirst        |
		| Last Name  | AddTestLast         |
		| Email      | AddTest@example.com |
		| Age        | 41                  |
		| Salary     | 123                 |
		| Department | Legal               |
	When User clicks on edit icon of specific record with First Name "AddTestFirst", Last Name "AddTestLast", and Email "AddTest@example.com"
	Then User should see last saved values


Scenario: Elements_WebTables_DeleteFunctionality_ShouldSucceed
	Given Click on "Elements" Sections
	And Click on "Web Tables" option
	When User clicks on delete icon of specific record with First Name "Alden", Last Name "Cantrell", and Email "alden@example.com"


Scenario: Elements_Buttons_AssertDifferentClickFunctionality_ShouldSucceed
	Given Click on "Elements" Sections
	And Click on "Buttons" option
	When Perform and assert "Double Click Me" action
	When Perform and assert "Right Click Me" action
	When Perform and assert "Click Me" action