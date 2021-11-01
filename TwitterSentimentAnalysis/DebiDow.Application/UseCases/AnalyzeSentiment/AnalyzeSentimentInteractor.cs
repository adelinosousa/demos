using System.Threading.Tasks;

namespace DebiDow.Application.UseCases.AnalyzeSentiment
{
    public class AnalyzeSentimentInteractor : IAnalyzeSentimentInteractor
    {
        private readonly IAnalyzeSentimentRepository _repository;

        public AnalyzeSentimentInteractor(IAnalyzeSentimentRepository repository)
        {
            _repository = repository;
        }

        public async Task<AnalyzeSentimentResponse> Handle(AnalyzeSentimentRequest request)
        {
            return new AnalyzeSentimentResponse
            {
                Samples = await _repository.AnalyzeSentiment(request.Samples)
            };
        }
    }
}
