using DebiDow.Application.UseCases.AnalyzeSentiment;
using Microsoft.Extensions.Configuration;
using Microsoft.ProjectOxford.Text.Sentiment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebiDow.Integration.UseCases.AnalyzeSentiment
{
    public class AnalyzeSentimentRepository : IAnalyzeSentimentRepository
    {
        private readonly IConfiguration configuration;

        public AnalyzeSentimentRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<List<AnalyzeSentimentSample>> AnalyzeSentiment(List<AnalyzeSentimentSample> samples)
        {
            try
            {
                var client = new SentimentClient(configuration["Azure:ApiKey"])
                {
                    Url = "https://westeurope.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment"
                };

                var request = new SentimentRequest();
                samples.ForEach(x => request.Documents.Add(new SentimentDocument
                {
                    Id = x.Id,
                    Text = x.Text,
                    Language = x.Language
                }));

                var response = await client.GetSentimentAsync(request);
                return response.Documents.Join(samples, x => x.Id, y => y.Id, 
                    (x, y) => {
                        y.Score = x.Score;
                        return y;
                    }).ToList();
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
