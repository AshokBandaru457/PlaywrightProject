using System.Threading.Tasks;
using Microsoft.Playwright;
using PlaywrightProject.Drivers;

namespace PlaywrightProject.Pages
{
    public class LoginPage
    {
        private IPage Page => Driver.Page;
        private readonly string _url = "http://eaapp.somee.com/";

        public async Task NavigateAsync() {
            await Page.GotoAsync(_url);
        } 
        private ILocator LoginLink => Page.Locator(selector:"text=Login");
        public async Task ClickLoginAsync() => await LoginLink.ClickAsync();

        public async Task EnterCredentialsAsync(string username, string password)
        {
            await Page.FillAsync(selector:"#UserName", username);
            await Page.FillAsync(selector: "#Password", password);
        }

        public async Task ClickLoginButtonAsync() => await Page.ClickAsync(selector: "text=loginIn");

        //public async Task<bool> IsLoggedInAsync()
        //{
        //    var elems = await Page.QuerySelectorAllAsync(".account, .logout");
        //    return elems.Count > 0;
        //}
    }
}
