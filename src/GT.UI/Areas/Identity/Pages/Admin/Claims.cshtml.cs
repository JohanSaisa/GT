using GT.Data.Data.GTIdentityDb.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace GT.UI.Areas.Identity.Pages.Admin
{
	public class ClaimsModel : PageModel
	{
		public ClaimsModel(UserManager<ApplicationUser> mgr)
		{
			UserManager = mgr;
		}

		public UserManager<ApplicationUser> UserManager { get; set; }

		[BindProperty(SupportsGet = true)]
		public string Id { get; set; }

		public IEnumerable<Claim> Claims { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			if (string.IsNullOrEmpty(Id))
			{
				//Redirect to NotFound
				return RedirectToPage("/");
			}
			ApplicationUser user = await UserManager.FindByIdAsync(Id);
			Claims = await UserManager.GetClaimsAsync(user);
			return Page();
		}

		public async Task<IActionResult> OnPostAddClaimAsync([Required] string type,
																												 [Required] string value)
		{
			ApplicationUser user = await UserManager.FindByIdAsync(Id);

			if (ModelState.IsValid)
			{
				var claim = new Claim(type, value);
				var result = await UserManager.AddClaimAsync(user, claim);
				if (!result.Succeeded)
				{
					foreach (var err in result.Errors)
					{
						ModelState.AddModelError(string.Empty, err.Description);
					}
				}
			}
			Claims = await UserManager.GetClaimsAsync(user);
			return RedirectToPage();
		}

		public async Task<IActionResult> OnPostEditClaimAsync([Required] string type,
																													[Required] string value,
																													[Required] string oldValue)
		{
			ApplicationUser user = await UserManager.FindByIdAsync(Id);
			if (ModelState.IsValid)
			{
				var claimNew = new Claim(type, value);
				var claimOld = new Claim(type, oldValue);
				var result = await UserManager.ReplaceClaimAsync(user, claimOld, claimNew);
			}
			Claims = await UserManager.GetClaimsAsync(user);
			return RedirectToPage();
		}

		public async Task<IActionResult> OnPostDeleteClaimAsync([Required] string type,
																														[Required] string value)
		{
			ApplicationUser user = await UserManager.FindByIdAsync(Id);
			if (ModelState.IsValid)
			{
				var claim = new Claim(type, value);
				var result = await UserManager.RemoveClaimAsync(user, claim);
			}
			Claims = await UserManager.GetClaimsAsync(user);
			return RedirectToPage();
		}
	}
}
