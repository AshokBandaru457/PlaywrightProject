Feature: Login functionality

@mytag
Scenario: Verify Login operation
	Given I navigate to the login page
	When I click on the login 
	And I enter username and password
		| username | password |
		| admin    | password |
	#And I click on login button
	#Then I should be logged in successfully
	