namespace DebiDow.Application.UseCases.AnalyzeSentiment
{
    public class AnalyzeSentimentSample
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Language { get; set; } = "en";
        public float Score { get; set; }
    }
}
