using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace dotNet_selenium_framework.pages;

public class BaseUiPage
{
    private readonly IWebDriver _driver;
    protected readonly Actions Actions;

    public BaseUiPage(IWebDriver driver)
    {
        _driver = driver;
        Actions = new Actions(_driver);
    }

    public void WaitForElementToBeClickable(By locator)
    {
        Wait([
            typeof(ElementClickInterceptedException),
            typeof(ElementNotInteractableException)
        ]).Until(ExpectedConditions.ElementToBeClickable(locator));
    }

    public IWebElement FindElement(By locator)
    {
        return Wait()
            .Until(dom => dom.FindElement(locator));
    }

    public IWebElement FindElement(By parentLocator, By locator)
    {
        return Wait()
            .Until(dom => dom.FindElement(parentLocator).FindElement(locator));
    }

    public IList<IWebElement> FindElements(By locator)
    {
        return Wait()
            .Until(dom => dom.FindElements(locator));
    }

    public void ClickOnElement(By locator)
    {
        FindElement(locator).Click();
    }

    public void ClickOnElement(By parentLocator, By locator)
    {
        FindElement(parentLocator, locator).Click();
    }

    public void Navigate(string url)
    {
        _driver.Navigate().GoToUrl(url);
        Thread.Sleep(1000);
    }

    private WebDriverWait Wait()
    {
        return Wait([]);
    }

    private WebDriverWait Wait(Type[] exceptionTypes)
    {
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
        if (exceptionTypes.Length > 0)
        {
            wait.IgnoreExceptionTypes(exceptionTypes);
        }

        return wait;
    }
}