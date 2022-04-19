using GT.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GT.UI.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[Route("")]
		public IActionResult Index()
		{
			return View();
		}

		[Route("Kontakt")]
		public IActionResult Contact()
		{
			return View();
		}

		//[Route("Annonser")]
		//public IActionResult Listings()
		//{
		//	return View();
		//}

		[Route("Info")]
		public IActionResult About()
		{
			return View();
		}

		[Route("Om-cookies")]
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
