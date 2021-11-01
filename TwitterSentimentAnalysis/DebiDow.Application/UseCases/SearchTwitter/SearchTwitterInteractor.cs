using System.Threading.Tasks;

namespace DebiDow.Application.UseCases.SearchTwitter
{
    public class SearchTwitterInteractor : ISearchTwitterInteractor
    {
        private readonly ISearchTwitterRepository _repository;

        public SearchTwitterInteractor(ISearchTwitterRepository repository)
        {
            _repository = repository;
        }

        public Task<SearchTwitterResponse> Handle(SearchTwitterRequest request)
        {
            var result = new SearchTwitterResponse();

            if (!string.IsNullOrEmpty(request.Search))
            {
                result.Messages = _repository.GetTweets(request.Search, request.MaximumNumberOfResults);
            }

            return Task.FromResult(result);
        }
    }
}
