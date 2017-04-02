using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace AarhusWebDevCoop.Controllers
{
	public class ContactFormSurfaceController : SurfaceController
	{
		// GET: Default
		public ActionResult Index() 
		{
			return PartialView("ContactFormPartialView", new ContactForm());
		}

		[HttpPost]
		public ActionResult HandleFormSubmit(ContactForm model)
		{
			if (ModelState.IsValid)
			{
				MailMessage message = new MailMessage();
				message.To.Add("viliusvig@gmail.com");
				message.Subject = model.Subject;
				message.From = new MailAddress(model.Email, model.Name);
				message.Body = model.Message;

				IContent msg = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "Message");
				msg.SetValue("senderName", model.Name);
				msg.SetValue("email", model.Email);
				msg.SetValue("subject", model.Subject);
				msg.SetValue("senderMessage", model.Message);

				// Save Message to Umbraco Back Office
				Services.ContentService.Save(msg);

				// Commented out due to gmail security reasons, it's working though
				//using (SmtpClient smtp = new SmtpClient())
				//{
				//	smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
				//	smtp.UseDefaultCredentials = false;
				//	smtp.EnableSsl = true;
				//	smtp.Host = "smtp.gmail.com";
				//	smtp.Port = 587;
				//	smtp.Credentials = new System.Net.NetworkCredential("viliusvig@gmail.com", "password");
				//	smtp.EnableSsl = true;

				//	//send email
				//	smtp.Send(message); 
				//}

				TempData["success"] = true;

				return RedirectToCurrentUmbracoPage();
			} 
			else 
			{
				return CurrentUmbracoPage();
			}
				
		}


	}
}