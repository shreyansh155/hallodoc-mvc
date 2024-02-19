using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
namespace SendMail.Controllers
{
    public class SendMailerController : Controller
    {
        //  
        // GET: /SendMailer/   
        public IActionResult SendMail()
        {
            return View();
        }


        [HttpPost]
        [HttpPost]
        public ViewResult Index(DataAccess.ViewModels.EmailModel _objModelMail)
        {
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(_objModelMail.To);
                mail.From = new MailAddress(_objModelMail.From);
                mail.Subject = _objModelMail.Subject;
                string Body = _objModelMail.Body;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("deserthawk087@gmail.com", "Cupboard"); // Enter seders User name and password       
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return View("SendMail", _objModelMail);
            }
            else
            {
                return View(_objModelMail);
            }
        }
    }
}
