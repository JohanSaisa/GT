namespace GT.UI.Models
{
	internal class AuthenticationConfiguration
	{
		internal string AccessTokenSecret { get; set; }
		internal string Issuer { get; set; }
		internal string Audience { get; set; }
	}
}
