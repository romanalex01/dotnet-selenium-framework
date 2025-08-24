namespace dotNet_selenium_framework.apis;

using RestSharp;

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

        var response = _client.Execute(request);
        Console.WriteLine("Execute [GET] discover movie: URL: {0}, response code: {1} {2}",
            response.ResponseUri, response.StatusCode, response.StatusDescription);
        Console.WriteLine("{0}", response.Content);
        return response;
    }
}