using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileSaveMail
{
    public class MailSend
    {
        private readonly string _smptServer = "smtp.gmail.com";
        private readonly int _port = 587;
        private readonly string _gmail = "artem2007yannurow@gmail.com";
        private readonly string _passwordmail = "";

        public async Task smptserververifi(string Email, string FilePath)
        {
            try
            {
                using var smptclient = new SmtpClient(_smptServer, _port)
                {
                    Credentials = new NetworkCredential(_gmail, _passwordmail),
                    EnableSsl = true
                };

                var smptmesseage = new MailMessage
                {
                    From = new MailAddress(_gmail),
                    Subject = "Ваш файл уже готов!",
                    Body = $"Файл: {System.IO.Path.GetFileName(FilePath)}\n\nОтправлено: {DateTime.Now}",
                    IsBodyHtml = false
                };

                smptmesseage.To.Add(Email);

                var attachment = new Attachment(FilePath);
                smptmesseage.Attachments.Add(attachment);

                await smptclient.SendMailAsync(smptmesseage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение в FileSaveMail -> MailSend -> smptserververifi" + ex.Message);
            }
        }
    }
}
