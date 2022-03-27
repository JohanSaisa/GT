using GT.Data.Data.GTIdentityDb;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GT.UI.Areas.Identity.Pages.Admin
{
	public class UsersModel : PageModel
	{
		public GTIdentityContext _DbCtx { get; set; }

		public IEnumerable<IdentityUser> Users { get; set; }
										= Enumerable.Empty<IdentityUser>();

		public UsersModel(GTIdentityContext dbCtx)
		{
			_DbCtx = dbCtx;
		}

		public void OnGet()
		{
			Users = _DbCtx.Users.ToList();
		}
	}
}
