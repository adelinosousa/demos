using System.Threading.Tasks;

namespace DebiDow.Application.UseCases.AnalyzeSentiment
{
    public interface IAnalyzeSentimentInteractor
    {
        Task<AnalyzeSentimentResponse> Handle(AnalyzeSentimentRequest request);
    }
}
