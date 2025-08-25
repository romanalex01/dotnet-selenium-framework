using dotNet_selenium_framework.apis;
using dotnet_selenium_framework.utils;

namespace dotNet_selenium_framework.tests;

public class TmdbTests : BaseTest
{
    private readonly string _sort = "Release Date Ascending";
    private readonly string[] _genres = ["Comedy", "Drama", "Family", "Romance"];
    private readonly string _fromDate = "1/1/1990";
    private readonly string _toDate = "1/1/2005";
    private readonly int[] _userScore = [8, 10];

    [Test]
    public void Task1_Filters()
    {
        HomePage.NavigateToHomePage();
        HomePage.ClickAcceptAllCookiesButton();
        HomePage.OpenPopularMoviesSearchPage();

        SearchPage.SortResultsBy(_sort);
        SearchPage.FilterByGenres(_genres);
        SearchPage.FilterByReleaseDateFrom(_fromDate);
        SearchPage.FilterByReleaseDateTo(_toDate);
        SearchPage.FilterByUserScore(_userScore);
        SearchPage.ClickSearchButton();

        Assert.Multiple(() =>
        {
            Assert.That(SearchPage.GetSortResultsBy(), Is.EqualTo(_sort));
            Assert.That(SearchPage.GetFilterByGenres(), Is.EqualTo(_genres));
            Assert.That(SearchPage.GetFilterByReleaseDateFrom(), Is.EqualTo(_fromDate));
            Assert.That(SearchPage.GetFilterByReleaseDateTo(), Is.EqualTo(_toDate));
            Assert.That(SearchPage.GetFilterByUserScore(), Is.EqualTo(_userScore));
        });
    }

    [Test]
    public void Task2_CheckTheFiltering()
    {
        HomePage.NavigateToHomePage();
        HomePage.ClickAcceptAllCookiesButton();
        HomePage.OpenPopularMoviesSearchPage();

        SearchPage.SortResultsBy(_sort);
        SearchPage.FilterByGenres(_genres);
        SearchPage.FilterByReleaseDateFrom(_fromDate);
        SearchPage.FilterByReleaseDateTo(_toDate);
        SearchPage.FilterByUserScore(_userScore);
        SearchPage.ClickSearchButton();

        var cardElements = SearchPage.GetMovies();
        var fromDate = DateTimeUtils.ConvertToDate(_fromDate);
        var toDate = DateTimeUtils.ConvertToDate(_toDate);
        var minScore = _userScore[0] * 10;
        var maxScore = _userScore[1] * 10;
        var dates = new List<DateOnly>();

        foreach (var cardElement in cardElements)
        {
            dates.Add(cardElement.Date());
            Assert.Multiple(() =>
            {
                Assert.That(cardElement.Date(), Is.InRange(fromDate, toDate));
                Assert.That(cardElement.Score(), Is.InRange(minScore, maxScore));
            });
        }

        Assert.That(dates, Is.Ordered); // Ascending
    }

    [Test]
    public void Task3_FiltersUIAndApi()
    {
        var tmdbApi = new TmdbApi("ce5e239860da25ec3cc5bc36ad8aab7d");

        HomePage.NavigateToHomePage();
        HomePage.ClickAcceptAllCookiesButton();
        HomePage.OpenPopularMoviesSearchPage();
        var uiMovies = SearchPage.GetMovies();
        
        var discoverMovieJson = tmdbApi.DiscoverMovie().Content;
        var apiMovies = ApiUtils.GetMovies(discoverMovieJson!);
        Assert.That(uiMovies, Is.EqualTo(apiMovies));
    }
}