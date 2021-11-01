using System.Collections.Generic;

namespace DebiDow.Application.UseCases.AnalyzeSentiment
{
    public class AnalyzeSentimentResponse
    {
        public List<AnalyzeSentimentSample> Samples { get; internal set; }
    }
}
