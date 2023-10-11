Feature: Assert Widgets Page
Assert different options available in Widgets page


@mytag
Scenario: Widgets_AutoComplete_ShouldSucceed
	Given User is on Main Page
	And Click on "Widgets" Sections
	And Click on "Auto Complete" option
	When User should be able to select multipleColor
	Then User should be able to select single color

Scenario: Widgets_AssertDatePickerUsingDatePicker_ShouldSucceed
	Given User is on Main Page
	And Click on "Widgets" Sections
	And Click on "Date Picker" option
	When User should be able to select "12/08/2023"

Scenario: Widgets_AssertToolTipFunctioanlity_ShouldSucceed
	Given User is on Main Page
	And Click on "Widgets" Sections
	And Click on "Tool Tips" option
	When Verify ToolTop functionality