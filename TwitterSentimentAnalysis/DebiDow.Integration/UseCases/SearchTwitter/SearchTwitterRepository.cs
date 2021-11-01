using DebiDow.Application.UseCases.SearchTwitter;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace DebiDow.Integration.UseCases.SearchTwitter
{
    public class SearchTwitterRepository : ISearchTwitterRepository
    {
        private readonly IConfiguration configuration;

        public SearchTwitterRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Dictionary<string, string> GetTweets(string search, int maximumNumberOfResults)
        {
            Auth.SetUserCredentials(
                configuration["Twitter:ConsumerKey"],
                configuration["Twitter:ConsumerSecret"],
                configuration["Twitter:UserAccessToken"],
                configuration["Twitter:UserAccessSecret"]);

            var user = User.GetAuthenticatedUser();

            var searchParameter = new SearchTweetsParameters(search)
            {
                //GeoCode = new GeoCode(-122.398720, 37.781157, 1, DistanceMeasure.Miles),
                Lang = LanguageFilter.English,
                SearchType = SearchResultType.Recent,
                MaximumNumberOfResults = maximumNumberOfResults,
                //Until = new DateTime(2015, 06, 02),
                //SinceId = 399616835892781056,
                //MaxId = 405001488843284480,
                Filters = TweetSearchFilters.Replies | TweetSearchFilters.Links
            };

            var matchingTweets = Search.SearchTweets(searchParameter);
            return matchingTweets.ToDictionary(k => k.IdStr, v => v.Text);
        }
    }
}
