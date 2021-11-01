using System.Collections.Generic;

namespace DebiDow.Application.UseCases.SearchTwitter
{
    public class SearchTwitterResponse
    {
        public Dictionary<string, string> Messages { get; set; }

        public SearchTwitterResponse()
        {
            Messages = new Dictionary<string, string>();
        }
    }
}
