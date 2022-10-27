using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Mail;
using static System.Net.WebRequestMethods;

namespace CRUD.Model.Models
{
    public class EmailHelper
    {
        public bool SendEmail(string userEmail, string confirmationLink)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("raj@gmail.com");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Confirm your email";
            mailMessage.IsBodyHtml = true;
            string baseUrl = "https://localhost:7080/api";

            string template = @"Hi ,<br/><br/>You have successfully created an account.
                                Please click on the link below to verify your email address and complete your registration.<br/>";

            template += "</br><a href= '" + $"{baseUrl}/Email/ConfirmEmail?token={confirmationLink}&email={userEmail}" + "'>Click here to activate your account.</a>";
            mailMessage.Body = template;
            mailMessage.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("rvpatel7719@gmail.com", "oacvsoqfivzigcan");
            client.Port = 587;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
    }
}