using dotnet_selenium_framework.model;
using Newtonsoft.Json.Linq;

namespace dotnet_selenium_framework.utils;

public static class ApiUtils
{
    public static string GetGenreIds(string genreMovieJson, string[] genreNames)
    {
        var result = "";
        var root = JObject.Parse(genreMovieJson);
        foreach (var t in genreNames)
        {
            var name = (string)root.SelectToken($"$.genres[?(@.name == '{t}')].id")!;
            if (result.Length == 0)
            {
                result += name;
            }
            else
            {
                result += $",{name}";
            }
        }

        return result;
    }

    public static List<Movie> GetMovies(string discoverMovieJson)
    {
        var result = new List<Movie>();
        JObject root = JObject.Parse(discoverMovieJson);
        JArray results = (JArray)root["results"]!;
        Console.WriteLine("API -------------------------------------------");
        foreach (var movie in results)
        {
            var title = (string)movie["title"]!;
            var date = (string)movie["release_date"]!;
            var vote = (float)movie["vote_average"]!;
            var score = (int)Math.Round(vote * 10);
            var item = new Movie(title, date, score.ToString());
            Console.WriteLine(item.ToString());
            result.Add(item);
        }

        Console.WriteLine("-----------------------------------------------");
        return result;
    }
}