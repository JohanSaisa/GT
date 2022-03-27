using GT.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GT.UI.Controllers.API
{
	[Route("[controller]")]
	[ApiController]
	public class UserTestController : Controller
	{
		[Authorize]
		[HttpGet]
		public List<string> DummyGet()
		{
			return new List<string> { "You are an authorized User." };
		}
	}
}
