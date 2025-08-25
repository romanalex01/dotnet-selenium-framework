using dotnet_selenium_framework.utils;

namespace dotnet_selenium_framework.model;

public class CardElement
{
    private readonly string _title;
    private readonly DateTime _date;
    private readonly int _score;

    public CardElement(string title, string date, string score)
    {
        _score = int.Parse(score);
        _title = title;
        _date = DateTimeUtils.ConvertToDate(date);
    }

    public string Title()
    {
        return _title;
    }

    public DateTime Date()
    {
        return _date;
    }

    public int Score()
    {
        return _score;
    }
}