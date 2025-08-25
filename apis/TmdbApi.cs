using dotnet_selenium_framework.model;
using RestSharp;

namespace dotNet_selenium_framework.apis;

public class TmdbApi
{
    private readonly string _apiKey;
    private readonly RestClient _client;

    public TmdbApi(string apiKey)
    {
        _apiKey = apiKey;
        var options = new RestClientOptions("https://api.themoviedb.org");
        _client = new RestClient(options);
    }

    public RestResponse GenreMovie()
    {
        var request = new RestRequest("/3/genre/movie/list");
        request.AddHeader("accept", "application/json");
        request.AddQueryParameter("api_key", _apiKey);
        request.AddQueryParameter("language", "en");
        var response = _client.Execute(request);
        Console.WriteLine("Execute [GET] genre movie: {0}", response.ResponseUri);
        return response;
    }

    public RestResponse DiscoverMovie()
    {
        return DiscoverMovie([]);
    }

    public RestResponse DiscoverMovie(Dictionary<string, string> queryParameters)
    {
        var request = new RestRequest("/3/discover/movie");
        request.AddHeader("accept", "application/json");
        request.AddQueryParameter("api_key", _apiKey);

        if (queryParameters.Any())
        {
            queryParameters.ToList().ForEach(dictionary =>
                request.AddQueryParameter(dictionary.Key, dictionary.Value));
        }

        var response = _client.Execute(request);
        Console.WriteLine("Execute [GET] discover movie: {0}", response.ResponseUri);
        return response;
    }
}