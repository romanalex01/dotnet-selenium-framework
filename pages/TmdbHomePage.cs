using OpenQA.Selenium;

namespace dotNet_selenium_framework.pages;

public class TmdbHomePage(IWebDriver driver) : BaseUiPage(driver)
{
    private readonly By _headerMoviesButtonLocator = By.XPath("//header//a[@aria-label='Movies']");
    private readonly By _headerPopularMoviesItemLocator = By.XPath("//ul/li/a[@aria-label='Popular']");
    private readonly By _acceptAllCookiesButtonLocator = By.XPath("//button[@id='onetrust-accept-btn-handler']");

    public void NavigateToHomePage()
    {
        Navigate("https://www.themoviedb.org/");
    }

    public void OpenPopularMoviesSearchPage()
    {
        ClickOnElement(_headerMoviesButtonLocator);
        ClickOnElement(_headerPopularMoviesItemLocator);
    }

    public void ClickAcceptAllCookiesButton()
    {
        WaitForElementToBeClickable(_acceptAllCookiesButtonLocator);
        ClickOnElement(_acceptAllCookiesButtonLocator);
    }
}