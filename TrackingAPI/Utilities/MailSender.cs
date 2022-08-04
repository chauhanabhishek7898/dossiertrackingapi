using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TrackingAPI.Utilities
{
    public class MailSender
    {
        public MailSender()
        {

        }

        public static void SendEmailWithOtp(string subject, string emailBody, string emailId, string ccEmailId = null, string bccEmailId = null)
        {
            // string MailBody = string.Empty;
            // if (verificationCodeType == "token") { MailBody = "<a href=\"http://localhost:4200/verified?vtoken=" + verificationCode + "\">Verify your Email</a>"; }
            // else if (verificationCodeType == "otp") { 
            //MailBody = $"Your One Time Pin (OTP) to login to Drome App is {verificationCode}. It is valid for 3 minutes."; //}
            try
            {
                MailMessage msg = new MailMessage(); msg.From = new MailAddress("admin@dossierexpress.com");
                msg.To.Add(new MailAddress(emailId));
                if (ccEmailId != null) { msg.CC.Add(new MailAddress(ccEmailId)); }
                if (bccEmailId != null) { msg.Bcc.Add(new MailAddress(bccEmailId)); }
                msg.Subject = subject;
                msg.IsBodyHtml = true; msg.Body = GenerateEmailOtpText(msg.Subject, emailBody);
                //msg.IsBodyHtml = true; msg.Body = "hi";
                SmtpClient smt = new SmtpClient(); smt.Host = "secure.emailsrvr.com";
                System.Net.NetworkCredential ntcd = new NetworkCredential(); smt.UseDefaultCredentials = true;
                ntcd.UserName = "admin@dossierexpress.com"; ntcd.Password = "admin120@Doss2022";
                smt.Credentials = ntcd; smt.EnableSsl = false; smt.Port = 587; smt.Send(msg);
            }
            catch (Exception ex) { throw ex; }
        }

        public static void SendEmailText(string subject, string body, string emailId = null, string ccEmailId = null, string bccEmailId = null, string bccEmailId2 = null)
        {
            try
            {
                MailMessage msg = new MailMessage(); msg.From = new MailAddress("admin@drome.health");
                if (emailId != null && emailId.Trim() != "") { msg.To.Add(new MailAddress(emailId)); }
                if (ccEmailId != null && ccEmailId.Trim() != "") { msg.CC.Add(new MailAddress(ccEmailId)); }
                if (bccEmailId != null && bccEmailId.Trim() != "") { msg.Bcc.Add(new MailAddress(bccEmailId)); }
                if (bccEmailId2 != null && bccEmailId2.Trim() != "") { msg.Bcc.Add(new MailAddress(bccEmailId2)); }
                msg.Subject = subject; msg.IsBodyHtml = true; msg.Body = GenerateEmailText(msg.Subject, body);
                SmtpClient smt = new SmtpClient(); smt.Host = "smtpout.secureserver.net";
                System.Net.NetworkCredential ntcd = new NetworkCredential(); smt.UseDefaultCredentials = true;
                ntcd.UserName = "admin@drome.health"; ntcd.Password = "Admin@drome";
                smt.Credentials = ntcd; smt.EnableSsl = false; smt.Port = 587; smt.Send(msg);
            }
            catch (Exception ex) { throw ex; }
        }

        public static void SendEmailTextEncounter(string subject, string body, string EmailId1 = null, string EmailId2 = null, string EmailId3 = null, string ccEmailId = null, string bccEmailId = null)
        {
            try
            {
                MailMessage msg = new MailMessage(); msg.From = new MailAddress("admin@drome.health");
                if (EmailId1 != null && EmailId1.Trim() != "") { msg.To.Add(new MailAddress(EmailId1)); }
                if (EmailId2 != null && EmailId2.Trim() != "") { msg.To.Add(new MailAddress(EmailId2)); }
                if (EmailId3 != null && EmailId3.Trim() != "") { msg.To.Add(new MailAddress(EmailId3)); }
                if (ccEmailId != null && ccEmailId.Trim() != "") { msg.Bcc.Add(new MailAddress(ccEmailId)); }
                if (bccEmailId != null && bccEmailId.Trim() != "") { msg.Bcc.Add(new MailAddress(bccEmailId)); }
                msg.Subject = subject; msg.IsBodyHtml = true; msg.Body = GenerateEmailText(msg.Subject, body);
                SmtpClient smt = new SmtpClient(); smt.Host = "smtpout.secureserver.net";
                System.Net.NetworkCredential ntcd = new NetworkCredential(); smt.UseDefaultCredentials = true;
                ntcd.UserName = "admin@drome.health"; ntcd.Password = "Admin@drome";
                smt.Credentials = ntcd; smt.EnableSsl = false; smt.Port = 587; smt.Send(msg);
            }
            catch (Exception ex) { throw ex; }
        }

        static string GenerateEmailOtpText(string subject, string messageBody)
        {
            var path = "EmailTemplate\\otp-mail-template.html";
            path = "wwwroot\\" + path;
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string message = System.IO.File.ReadAllText(path);
            message = message.Replace("{{subject}}", subject);
            message = message.Replace("{{messageBody}}", messageBody);
            return message;
        }

        static string GenerateEmailText(string subject, string bodyText)
        {
            var path = "EmailTemplate\\mail-template.html";
            path = "wwwroot\\" + path;
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string message = System.IO.File.ReadAllText(path);
            message = message.Replace("{{subject}}", subject);
            message = message.Replace("{{body}}", bodyText);
            return message;

        }

        public static void SendEmailAttachment(string subject, string body, string emailId, string attachmentPath, string ccEmailId = null, string bccEmailId = null)
        {
            try
            {
                string filePath = "wwwroot\\" + attachmentPath;
                string fileName = Path.GetFileName(filePath);
                byte[] bytes = File.ReadAllBytes(filePath);
                MailMessage msg = new MailMessage(); msg.From = new MailAddress("admin@drome.health");
                if (emailId != null && emailId.Trim() != "") { msg.To.Add(new MailAddress(emailId)); }
                if (ccEmailId != null && ccEmailId.Trim() != "") { msg.Bcc.Add(new MailAddress(ccEmailId)); }
                if (bccEmailId != null && bccEmailId.Trim() != "") { msg.Bcc.Add(new MailAddress(bccEmailId)); }
                msg.Subject = subject; msg.IsBodyHtml = true; msg.Body = GenerateEmailText(msg.Subject, body);
                msg.Attachments.Add(new Attachment(new MemoryStream(bytes), fileName));
                SmtpClient smt = new SmtpClient(); smt.Host = "smtpout.secureserver.net";
                System.Net.NetworkCredential ntcd = new NetworkCredential(); smt.UseDefaultCredentials = true;
                ntcd.UserName = "admin@drome.health"; ntcd.Password = "Admin@drome";
                smt.Credentials = ntcd; smt.EnableSsl = false; smt.Port = 587; smt.Send(msg);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
