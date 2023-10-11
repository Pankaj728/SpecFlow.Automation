Feature: Assert Widgets Page
Assert different options available in Widgets page


@mytag
Scenario: Widgets_AutoComplete_ShouldSucceed
	Given User is on Main Page
	And Click on "Interactions" Sections
	And Click on "Sortable" option
	When User should be able to select multipleColor
	Then User should be able to select single color

