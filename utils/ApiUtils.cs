using dotnet_selenium_framework.model;
using Newtonsoft.Json.Linq;

namespace dotnet_selenium_framework.utils;

public class ApiUtils
{
    public static List<Movie> GetMovies(string discoverMovieJson)
    {
        var result = new List<Movie>();
        JObject root = JObject.Parse(discoverMovieJson);
        JArray results = (JArray)root["results"]!;
        Console.WriteLine("JSON ==========================================");
        foreach (var movie in results)
        {
            var title = (string)movie["title"]!;
            var date = (string)movie["release_date"]!;
            var vote = (float)movie["vote_average"]!;
            var score = (int)Math.Round(vote * 10);;
            var item = new Movie(title, date, score.ToString());
            Console.WriteLine($"title: {title}, date: {date}, score: {score}, vote: {vote}");
            Console.WriteLine(item.ToString());
            result.Add(item);
        }

        Console.WriteLine("==========================================");
        return result;
    }
}