using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MyWarmPetWeb.Utils
{
    public class EmailSender
    {
        private const String API_KEY = "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";
        //private const String API_KEY = "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";

        public void Send(String toEmail)
        {
            String subject = "Congratulation";
            String contents = "You have successfully booked a time with a vet.";
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("Lu_Zhou05@hotmail.com", "FIT5032 My Warm Pet");
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public void SendToAll(String subject, String contents, List<String> emails, HttpPostedFileBase postedFileBase)
        {
           
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("Lu_Zhou05@hotmail.com", "FIT5032 My Warm Pet");
            var tos = new List<EmailAddress>();
            foreach(var item in emails)
            {
                var tempEmail = new EmailAddress(item, "");
                tos.Add(tempEmail);
            }
            //var to = new EmailAddress(toEmail, "");
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, plainTextContent, htmlContent);

            if (postedFileBase != null && postedFileBase.ContentLength > 0)
            {
                string theFileName = Path.GetFileName(postedFileBase.FileName);
                byte[] fileBytes = new byte[postedFileBase.ContentLength];
                using (BinaryReader theReader = new BinaryReader(postedFileBase.InputStream))
                {
                    fileBytes = theReader.ReadBytes(postedFileBase.ContentLength);
                }
                string dataAsString = Convert.ToBase64String(fileBytes);
                msg.AddAttachment(theFileName, dataAsString);
            }

            var response = client.SendEmailAsync(msg);
        }
    }
}