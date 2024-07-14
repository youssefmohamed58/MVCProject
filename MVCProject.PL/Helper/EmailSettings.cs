using System.Net;
using System.Net.Mail;
using Project.DAL.Models;

namespace MVCProject.PL.Helper
{
	public static class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			var client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("e.ahmed2684@gmail.com", "yzkklvbqrmeitnrh");
			client.Send("e.ahmed2684@gmail.com", email.To, email.Subject, email.Body);
		}
	}
}
