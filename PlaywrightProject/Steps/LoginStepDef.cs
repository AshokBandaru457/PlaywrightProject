using System;
using System.Linq;
using System.Threading.Tasks;
using PlaywrightProject.Pages;

namespace PlaywrightProject.StepDefinitions
{
    [Binding]
    public class LoginStepDef
    {
        private readonly LoginPage _loginPage;

        public LoginStepDef(LoginPage loginPage)
        {
            _loginPage = loginPage;
        }

        [Given("I navigate to the login page")]
        public async Task GivenINavigateToTheLoginPage()
        {
            await _loginPage.NavigateAsync();
        }

        [When("I click on the login")]
        public async Task WhenIClickOnTheLogin()
        {
            await _loginPage.ClickLoginAsync();
        }

        [When("I enter username and password")]
        public async Task WhenIEnterUsernameAndPassword(Table table)
        {
            var row = table.Rows.First();
            var username = row.ContainsKey("username") ? row["username"] : row.Values.First();
            var password = row.ContainsKey("password") ? row["password"] : row.Values.Skip(1).FirstOrDefault() ?? string.Empty;

            await _loginPage.EnterCredentialsAsync(username, password);
        }

        [When("I click on login button")]
        public async Task WhenIClickOnLoginButton()
        {
            await _loginPage.ClickLoginButtonAsync();
        }

        //[Then("I should be logged in successfully")]
        //public async Task ThenIShouldBeLoggedInSuccessfully()
        //{
        //    var loggedIn = await _loginPage.IsLoggedInAsync();
        //    if (!loggedIn)
        //        throw new Exception("Login verification failed - expected logged in state.");
        //}
    }
}
