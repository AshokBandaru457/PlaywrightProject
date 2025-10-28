using System;
using System.Threading.Tasks;
using PlaywrightProject.Drivers;

namespace PlaywrightProject.Hooks
{
    [Binding]
    public sealed class DriverHooks
    {
        [BeforeScenario]
        public async Task BeforeScenarioAsync()
        {
            var browser = Environment.GetEnvironmentVariable("BROWSER") ?? "chromium";
            var headlessEnv = Environment.GetEnvironmentVariable("HEADLESS") ?? "true";
            var headless = bool.TryParse(headlessEnv, out var h) && h;

            await Driver.StartAsync(browser, headless);
        }

        [AfterScenario]
        public async Task AfterScenarioAsync()
        {
            await Driver.StopAsync();
        }
    }
}