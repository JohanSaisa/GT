using GT.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GT.UI.Controllers.API
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdminTestController : Controller
	{
		[Authorize(Policy = "AdminPolicy")]
		[HttpGet]
		public List<string> DummyPost()
		{
			return new List<string> { "You are an authorized Admin." };
		}
	}
}
