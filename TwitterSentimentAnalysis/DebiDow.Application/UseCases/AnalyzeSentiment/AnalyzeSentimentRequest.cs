using System.Collections.Generic;

namespace DebiDow.Application.UseCases.AnalyzeSentiment
{
    public class AnalyzeSentimentRequest
    {
        public List<AnalyzeSentimentSample> Samples { get; set; } = new List<AnalyzeSentimentSample>();
    }
}
