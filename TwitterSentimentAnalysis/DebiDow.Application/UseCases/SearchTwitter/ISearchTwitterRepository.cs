using System.Collections.Generic;

namespace DebiDow.Application.UseCases.SearchTwitter
{
    public interface ISearchTwitterRepository
    {
        Dictionary<string, string> GetTweets(string search, int maximumNumberOfResults);
    }
}
