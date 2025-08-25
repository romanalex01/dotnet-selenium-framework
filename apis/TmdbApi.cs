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
            queryParameters.ToList().ForEach(x =>
                request.AddQueryParameter(x.Key, x.Value));
        }

        return _client.Execute(request);
    }
}