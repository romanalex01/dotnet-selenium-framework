using dotnet_selenium_framework.utils;

namespace dotnet_selenium_framework.model;

public class Movie
{
    private readonly string _title;
    private readonly DateOnly _date;
    private readonly int _score;

    public Movie(string title, string date, string score)
    {
        _title = title;
        _date = DateTimeUtils.ConvertToDate(date);
        _score = int.Parse(score);
    }

    public string Title()
    {
        return _title;
    }

    public DateOnly Date()
    {
        return _date;
    }

    public int Score()
    {
        return _score;
    }

    public override string ToString()
    {
        return $"title: {_title}, date: {_date},  score: {_score}";
    }
}