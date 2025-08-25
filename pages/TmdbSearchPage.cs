using OpenQA.Selenium;

namespace dotNet_selenium_framework.pages;

public class TmdbSearchPage(IWebDriver driver) : BaseUiPage(driver)
{
    private readonly By _sortPanelCardLocator = By.XPath("//div[contains(@class,'filter_panel card')][.//h2[text()='Sort']]");
    private readonly By _filterPanelCardLocator = By.XPath("//div[contains(@class,'filter_panel card')][.//h2[text()='Filters']]");
    private readonly By _releaseDatesLocator = By.XPath("//div[@class='filter'][.//h3[text()='Release Dates']]");
    private readonly By _calendarPopupReleaseDateGteLocator = By.XPath("//div[@id='release_date_gte_dateview']");
    private readonly By _calendarPopupReleaseDateLteLocator = By.XPath("//div[@id='release_date_lte_dateview']");
    private readonly By _genresLocator = By.XPath("//div[@class='filter'][.//h3[text()='Genres']]");
    private readonly By _userScoreLocator = By.XPath("//div[@class='filter'][.//h3[text()='User Score']]");


    public void SortResultsBy(string text)
    {
        OpenFilterPanelCard(_sortPanelCardLocator);

        var sortPanelCardButtonLocator = By.XPath(".//button");
        ClickOnElement(_sortPanelCardLocator, sortPanelCardButtonLocator);

        var sortPanelCardOptionLocator = By.XPath("//ul[@id='sort_by_listbox']/li");
        IList<IWebElement> options = FindElements(sortPanelCardOptionLocator);

        foreach (IWebElement option in options)
        {
            if (option.Text.Trim().Equals(text, StringComparison.OrdinalIgnoreCase))
            {
                option.Click();
                return;
            }

            Actions.SendKeys(Keys.ArrowDown).Perform();
        }
    }

    public string GetSortResultsBy()
    {
        OpenFilterPanelCard(_sortPanelCardLocator);
        var sortValueLocator = By.XPath(".//span[@class='k-input-value-text']");
        return FindElement(_sortPanelCardLocator)
            .FindElement(sortValueLocator).Text;
    }

    public void FilterByGenres(string[] genres)
    {
        OpenFilterPanelCard(_filterPanelCardLocator);

        foreach (string genre in genres)
        {
            var genreLocator = By.XPath(".//a[text()='" + genre + "']");
            var genreElement = FindElement(_genresLocator)
                .FindElement(genreLocator);
            Actions.MoveToElement(genreElement).Perform();
            Actions.ScrollByAmount(0, 200).Perform();
            genreElement.Click();
        }
    }

    public List<string> GetFilterByGenres()
    {
        OpenFilterPanelCard(_filterPanelCardLocator);

        var selectedGenresLocator = By.XPath(".//ul[@id='with_genres']/li[@class='selected']/a");
        var selectedGenresElements = FindElement(_genresLocator)
            .FindElements(selectedGenresLocator);
        return selectedGenresElements.Select(element => element.Text).ToList();
    }

    public void FilterByReleaseDateFrom(string from)
    {
        OpenFilterPanelCard(_filterPanelCardLocator);
        if (from.Trim().Length > 0)
        {
            FillOnCalendar(_releaseDatesLocator, _calendarPopupReleaseDateGteLocator, "from", from);
        }
    }

    public string GetFilterByReleaseDateFrom()
    {
        OpenFilterPanelCard(_filterPanelCardLocator);
        var inputLocator = By.XPath(".//input[@id='release_date_gte']");
        return FindElement(_releaseDatesLocator)
            .FindElement(inputLocator).GetAttribute("value")!;
    }

    public void FilterByReleaseDateTo(string to)
    {
        OpenFilterPanelCard(_filterPanelCardLocator);
        if (to.Trim().Length > 0)
        {
            FillOnCalendar(_releaseDatesLocator, _calendarPopupReleaseDateLteLocator, "to", to);
        }
    }

    public string GetFilterByReleaseDateTo()
    {
        OpenFilterPanelCard(_filterPanelCardLocator);
        var inputLocator = By.XPath(".//input[@id='release_date_lte']");
        return FindElement(_releaseDatesLocator)
            .FindElement(inputLocator).GetAttribute("value")!;
    }

    public void FilterByUserScore(int min, int max)
    {
        OpenFilterPanelCard(_filterPanelCardLocator);

        if ((min is >= 0 and <= 10)
            && (max is >= 0 and <= 10)
            && (min < max))
        {
            var scoreLocatorFormat = ".//div[@class='k-slider-track']//span[{0}]";
            var minScoreLocator = By.XPath(String.Format(scoreLocatorFormat, 1));
            var maxScoreLocator = By.XPath(String.Format(scoreLocatorFormat, 2));
            UserScoreSetupScore(maxScoreLocator, max);
            UserScoreSetupScore(minScoreLocator, min);
        }
    }

    public int[] GetFilterByUserScore()
    {
        OpenFilterPanelCard(_filterPanelCardLocator);
        var scoreLocator = By.XPath(".//div[@class='k-slider-track']//span");
        var scoreElements = FindElement(_userScoreLocator)
            .FindElements(scoreLocator);

        var scoreResult = new int [2];
        scoreResult[0] = int.Parse(scoreElements[0].GetAttribute("aria-valuenow")!);
        scoreResult[1] = int.Parse(scoreElements[1].GetAttribute("aria-valuenow")!);
        return scoreResult;
    }

    public void ClickSearchButton()
    {
        var searchButtonLocator = By.XPath("//div[contains(@class,'apply small')]//a[text()='Search']");
        Actions.MoveToElement(FindElement(searchButtonLocator));
        ClickOnElement(searchButtonLocator);
    }


    private void OpenFilterPanelCard(By locator)
    {
        var classAttribute = FindElement(locator).GetAttribute("class");
        if (classAttribute != null && classAttribute.Contains("closed"))
        {
            ClickOnElement(locator);
        }
    }

    private void FillOnCalendar(By filterLocation, By calendarPopupLocator, string label, string date)
    {
        OpenCalendarPopup(filterLocation, label);
        CalendarPopupClickToday(calendarPopupLocator);

        OpenCalendarPopup(filterLocation, label);
        CalendarPopupNavigateToCenturyView(calendarPopupLocator);
        CalendarPopupClickPreviousButton(calendarPopupLocator);

        var day = int.Parse(date.Split("/")[1]);
        var month = int.Parse(date.Split("/")[0]);
        var year = int.Parse(date.Split("/")[2]);
        CalendarPopupSelectDecate(calendarPopupLocator, year);
        CalendarPopupSelectYear(calendarPopupLocator, year);
        CalendarPopupSelectMonth(calendarPopupLocator, month);
        CalendarPopupSelectDay(calendarPopupLocator, day);
    }

    private void OpenCalendarPopup(By filterLocation, string label)
    {
        var dateLocator = By.XPath(".//div[@class='year_column'][./span[text()='" + label + "']]");
        var buttonLocator = By.XPath(".//button");
        var openCalendarButtonElement = FindElement(filterLocation)
            .FindElement(dateLocator)
            .FindElement(buttonLocator);

        Actions.ScrollToElement(openCalendarButtonElement).Perform();
        Actions.ScrollByAmount(0, 200).Perform();
        openCalendarButtonElement.Click();
    }

    private void CalendarPopupClickToday(By calendarPopupLocator)
    {
        var todayButtonLocator = By.XPath(".//button[./span[text()='Today']]");
        FindElement(calendarPopupLocator)
            .FindElement(todayButtonLocator).Click();
    }

    private void CalendarPopupNavigateToCenturyView(By calendarPopupLocator)
    {
        var navUpButtonLocator = By.XPath(".//button[@data-action='nav-up']");
        var navUpButtonElement = FindElement(calendarPopupLocator)
            .FindElement(navUpButtonLocator);
        while (navUpButtonElement.GetAttribute("aria-disabled")!.Equals("false"))
        {
            navUpButtonElement.Click();
        }
    }

    private void CalendarPopupClickPreviousButton(By calendarPopupLocator)
    {
        var prevButtonLocator = By.XPath(".//button[@data-action='prev']");
        var prevButtonElement = FindElement(calendarPopupLocator)
            .FindElement(prevButtonLocator);
        while (prevButtonElement.GetAttribute("aria-disabled")!.Equals("false"))
        {
            prevButtonElement.Click();
        }
    }

    private void CalendarPopupClickNextButton(By calendarPopupLocator)
    {
        var nextButtonLocator = By.XPath(".//button[@data-action='next']");
        var nextButtonElement = FindElement(calendarPopupLocator)
            .FindElement(nextButtonLocator);
        nextButtonElement.Click();
    }

    private void CalendarPopupSelectDecate(By calendarPopupLocator, int year)
    {
        var decatesLocator = By.XPath(".//tbody//td[not(contains(@class,'k-empty'))]");
        var decatesElements = FindElement(calendarPopupLocator)
            .FindElements(decatesLocator);

        bool foundYear = false;
        foreach (var decatesTextElement in decatesElements)
        {
            var yearsArray = decatesTextElement.Text.Trim().Split("-");
            var minYear = int.Parse(yearsArray[0]);
            var maxYear = int.Parse(yearsArray[1]);
            if (year < minYear || year > maxYear) continue;
            decatesTextElement.Click();
            foundYear = true;
            break;
        }

        if (foundYear) return;
        CalendarPopupClickNextButton(calendarPopupLocator);
        CalendarPopupSelectDecate(calendarPopupLocator, year);
    }

    private void CalendarPopupSelectYear(By calendarPopupLocator, int year)
    {
        var yearLocator = By.XPath(".//tbody//td[./span[text()='" + year + "']]");
        FindElement(calendarPopupLocator)
            .FindElement(yearLocator)
            .Click();
    }

    private void CalendarPopupSelectMonth(By calendarPopupLocator, int month)
    {
        var monthsLocator = By.XPath(".//tbody//td[not(contains(@class,'k-empty'))]");
        FindElement(calendarPopupLocator)
            .FindElements(monthsLocator)[(month - 1)].Click();
    }

    private void CalendarPopupSelectDay(By calendarPopupLocator, int day)
    {
        Thread.Sleep(1000);
        var dayLocator = By.XPath(".//tbody//td[not(contains(@class,'k-other-month'))][./span[text()='" + day + "']]");
        FindElement(calendarPopupLocator)
            .FindElement(dayLocator).Click();
    }

    private void UserScoreSetupScore(By scoreLocator, int value)
    {
        var scoreElement = FindElement(_userScoreLocator)
            .FindElement(scoreLocator);
        Actions.MoveToElement(scoreElement).Perform();
        Actions.ScrollByAmount(0, 200).Perform();
        scoreElement.Click();

        while (!scoreElement.GetAttribute("aria-valuenow")!.Equals("0"))
        {
            Actions.SendKeys(Keys.Left).Perform();
            Thread.Sleep(100);
        }

        for (int i = 0; i < value; i++)
        {
            Actions.SendKeys(Keys.Right).Perform();
            Thread.Sleep(100);
        }
    }
}