using GT.Core.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GT.Core.Services.Impl
{
	public class EmailSender : IEmailSender
	{
		private readonly IConfiguration _configuration;

		public EmailSender(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var gtEmail = _configuration["Authentication:Gmail:ClientId"];
			var gtPassword = _configuration["Authentication:Gmail:Secret"];

			MailMessage Msg = new MailMessage(gtEmail, email);
			Msg.Subject = subject;
			Msg.Body = htmlMessage;
			Msg.IsBodyHtml = true;

			SmtpClient smtp = new SmtpClient();
			smtp.Host = "smtp.gmail.com";
			smtp.Port = 587;
			smtp.UseDefaultCredentials = false;
			smtp.EnableSsl = true;
			smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

			NetworkCredential MyCredentials = new NetworkCredential(gtEmail, gtPassword);
			smtp.Credentials = MyCredentials;
			smtp.Send(Msg);
		}
	}
}
