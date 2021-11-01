using System.Web.Mvc;
using TextMatch.Common.Models;
using TextMatch.Common.Services;

namespace TextMatch.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult FindMatches(string text, string subtext)
        {
            return PartialView(new MatchResultsViewModel(TextMatchService.FindMatches(text, subtext)));
        }
    }
}