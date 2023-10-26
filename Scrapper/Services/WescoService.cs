using PuppeteerSharp;
using Scrapper.Contracts;

namespace Scrapper.Services;

public class WescoService : IWescoService
{
    private readonly IWebHostEnvironment _env;

    public WescoService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task TakeScreenshotAsync()
    {
        using var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();

        var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true
        });

        var page = await browser.NewPageAsync();
        await page.GoToAsync("http://www.google.com");

        var outputFile = Path.Combine(_env.WebRootPath, "screenshots", "google.png");
        await page.ScreenshotAsync(outputFile);
    }
}