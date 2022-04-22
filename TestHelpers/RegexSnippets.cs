using System.Text.RegularExpressions;

namespace TestHelpers
{
	public static class RegexSnippets
	{
		public static string GetGuidRegex()
		{
			var regexString = @"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$";

			// Verify that regex is valid
			if (Regex.IsMatch(Guid.NewGuid().ToString(), regexString))
			{
				return regexString;
			}
			else
			{
				throw new Exception("Variable 'regexString' is not providing a valid regex pattern. " +
					"Re Review method RegexSnippets.GetGuidRegex().");
			}
		}
	}
}
