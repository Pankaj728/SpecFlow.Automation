Feature: Assert Alerts Frame Windows Page
Assert different options available in AlertsFrameWindowsPage page


@mytag
Scenario: AlertsFrameWindows_BrowserWindows_AssertFunctionality_ShouldSucceed
	Given User is on Main Page
	And Click on "Alerts, Frame & Windows" Sections
	And Click on "Browser Windows" option
	When Perform and assert "New Tab" Window action
	When Perform and assert "New Window" Window action
	When Perform and assert "New Window Message" Window action


Scenario: AlertsFrameWindows_Alerts_AssertFunctionality_ShouldSucceed
	Given User is on Main Page
	And Click on "Alerts, Frame & Windows" Sections
	And Click on "Alerts" option
	When Perform and assert "Normal Alert" Alert action
	When Perform and assert "Delayed Alert" Alert action
	When Perform and assert "Confirm Box" Alert action
	When Perform and assert "Prompt Box" Alert action