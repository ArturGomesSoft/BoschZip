using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;

namespace BoschZip
{
    public class SMTPSender : ISender
    {
        SMTPSettings settings;
        string recipientAddress = string.Empty;

        public SMTPSender(SMTPSettings settings, string recipientAddress)
        {
            this.settings = settings;
            this.recipientAddress = recipientAddress;
        }

        private bool IsValidEmail(string address)
        {
            string pat = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex reg = new Regex(pat);
            return reg.IsMatch(address);
        }

        private string GetMessageSubject()
        {
            return "File from Bosch";
        }

        private string GetMessageBody()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"Here's a file automatically sent by Bosch.");
            sb.AppendLine(@"Please, check attachment.");
            sb.AppendLine(@"Do not reply to this message.");
            sb.AppendLine(@" ");
            sb.AppendLine(@"Bosch Zipper");
            return sb.ToString();
        }

        public ResultObj Send(string fileToSend)
        {
            ResultObj retVal = new ResultObj();

            if (string.IsNullOrEmpty(fileToSend) || !File.Exists(fileToSend))
            {
                retVal.ResultType = ResultTypeEnum.Failure;
                retVal.Message.AppendLine("Invalid file to be sent.");
                return retVal;
            }

            if (!IsValidEmail(this.recipientAddress))
            {
                retVal.ResultType = ResultTypeEnum.Failure;
                retVal.Message.AppendLine("Invalid recipient email address.");
                return retVal;
            }

            Attachment fileAttachment = null;
            try
            {
                MailAddress senderAddress = new MailAddress(settings.SenderAddress, settings.SenderName);
                MailAddress recipientAddres = new MailAddress(this.recipientAddress);

                MailMessage message = new MailMessage(senderAddress, recipientAddres);
                message.Subject = GetMessageSubject();
                message.Body = GetMessageBody();

                fileAttachment = new Attachment(fileToSend, MediaTypeNames.Application.Zip);
                message.Attachments.Add(fileAttachment);

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = settings.SmtpHost; // "smtp.gmail.com";
                    smtp.Port = settings.SmtpPort; // 587;
                    smtp.DeliveryMethod = settings.SmtpDeliverymethod; // SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = settings.SmtpUserDefaultCredentials; // false;
                    smtp.EnableSsl = settings.SmtpEnableSsl; // true;
                    smtp.Credentials = new NetworkCredential(settings.SenderAddress, settings.Password);

                    smtp.Send(message);
                }

            }
            catch (Exception ex)
            {
                retVal.ResultType = ResultTypeEnum.Failure;
                retVal.Message.AppendLine(ex.Message);
            }
            finally
            {
                if (fileAttachment != null)
                {
                    fileAttachment.Dispose();
                }
            }
            return retVal;
        }

    }
}
