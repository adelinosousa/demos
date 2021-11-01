using System.Collections.Generic;
using System.Threading.Tasks;

namespace DebiDow.Application.UseCases.AnalyzeSentiment
{
    public interface IAnalyzeSentimentRepository
    {
        Task<List<AnalyzeSentimentSample>> AnalyzeSentiment(List<AnalyzeSentimentSample> samples);
    }
}
