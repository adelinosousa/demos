using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DebiDow.Models;
using DebiDow.Application.UseCases.AnalyzeSentiment;
using DebiDow.Application.UseCases.SearchTwitter;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace DebiDow.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnalyzeSentimentInteractor _analyzeSentimentInteractor;
        private readonly ISearchTwitterInteractor _searchTwitterInteractor;

        public HomeController(IAnalyzeSentimentInteractor analyzeSentimentInteractor,
            ISearchTwitterInteractor searchTwitterInteractor)
        {
            _analyzeSentimentInteractor = analyzeSentimentInteractor;
            _searchTwitterInteractor = searchTwitterInteractor;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string search)
        {
            var result = new List<AnalyzeSentimentSample>();

            var twitterResult = await _searchTwitterInteractor.Handle(new SearchTwitterRequest
            {
                Search = search,
                MaximumNumberOfResults = 5
            });

            if (twitterResult.Messages.Any())
            {
                var sentimentResults = await _analyzeSentimentInteractor.Handle(new AnalyzeSentimentRequest
                {
                    Samples = twitterResult.Messages.Select(m => new AnalyzeSentimentSample
                    {
                        Id = m.Key,
                        Text = m.Value
                    }).ToList()
                });

                result = sentimentResults.Samples;
            }

            return View(result);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
