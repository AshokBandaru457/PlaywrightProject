using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace PlaywrightProject.Drivers
{
    public static class Driver
    {
        private static IPlaywright? _playwright;
        private static IBrowser? _browser;
        private static IBrowserContext? _context;
        private static IPage? _page;

        /// <summary>
        /// The active Playwright page. Throws if Playwright hasn't been started.
        /// </summary>
        public static IPage Page => _page ?? throw new InvalidOperationException("Playwright not started. Call Driver.StartAsync(...) in your test setup.");

        /// <summary>
        /// Start Playwright and open a browser + page. Default browser: chromium.
        /// </summary>
        public static async Task StartAsync(string browser = "chromium", bool headless = true)
        {
            if (_playwright != null) return; // already started

            _playwright = await Playwright.CreateAsync();

            switch (browser.ToLowerInvariant())
            {
                case "firefox":
                    _browser = await _playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = headless });
                    break;
                case "webkit":
                case "safari":
                    _browser = await _playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions { Headless = headless });
                    break;
                default:
                    // chromium covers Chrome and Edge-like browsers
                    _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                    {
                        Headless = false,
                        Args = new[] { "--start-maximized" }
                    });
                    break;
            }

            // Create a context with no fixed viewport to let the browser window size be used
            _context = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = null
            });

            _page = await _context.NewPageAsync();

            // sensible defaults
            _page.SetDefaultTimeout(10_000);
            _page.SetDefaultNavigationTimeout(30_000);
        }

        /// <summary>
        /// Creates a fresh context + page (useful when tests require isolated contexts).
        /// </summary>
        public static async Task<IPage> NewContextPageAsync(BrowserNewContextOptions? options = null)
        {
            if (_browser == null) throw new InvalidOperationException("Browser not started. Call Driver.StartAsync first.");
            var ctx = await _browser.NewContextAsync(options);
            return await ctx.NewPageAsync();
        }

        /// <summary>
        /// Gracefully stops Playwright and disposes resources.
        /// </summary>
        public static async Task StopAsync()
        {
            if (_page != null)
            {
                try { await _page.CloseAsync(); } catch { /* ignore */ }
                _page = null;
            }

            if (_context != null)
            {
                try { await _context.CloseAsync(); } catch { /* ignore */ }
                _context = null;
            }

            if (_browser != null)
            {
                try { await _browser.CloseAsync(); } catch { /* ignore */ }
                _browser = null;
            }

            if (_playwright != null)
            {
                try { _playwright.Dispose(); } catch { /* ignore */ }
                _playwright = null;
            }
        }
    }
}