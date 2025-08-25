using dotNet_selenium_framework.pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace dotNet_selenium_framework.tests;

public class BaseTest
{
    private IWebDriver _driver;
    protected TmdbHomePage HomePage;
    protected TmdbSearchPage SearchPage;

    [SetUp]
    public void SetUp()
    {
        var options = new ChromeOptions();
        //options.AddArgument("--headless"); // run headless
        _driver = new ChromeDriver(options);
        _driver.Manage().Window.Maximize();
        _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
        
        HomePage = new TmdbHomePage(_driver);
        SearchPage = new TmdbSearchPage(_driver);
    }

    [TearDown]
    public void TearDown()
    {
        _driver.Quit();
        _driver.Dispose();
        _driver = null!;
    }
}