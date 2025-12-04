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
    public class MailSendGmail
    {
        private readonly string _smptServer = "smtp.gmail.com";
        private readonly int _port = 587;
        private readonly string _gmail = "artem2007yannurow@gmail.com";
        private readonly string _passwordmail = "mdyh mrza nlki drry";

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
                MessageBox.Show("Возникло исключение в FileSaveMail -> MailSendGmail -> smptserververifi" + ex.Message);
            }
        }
    }

    public class MailSendYandex
    {
        private readonly string _smptServer = "smtp.yandex.ru";
        private readonly int _port = 465;
        private readonly string _gmail = "yannuroff.a@yandex.ru";
        private readonly string _passwordmail = "zvkjyxhhavkbwmca";

        public async Task smptserververifi(string Email, string FilePath)
        {
            try
            {
                using var smptclient = new SmtpClient(_smptServer, _port)
                {
                    Credentials = new NetworkCredential(_gmail, _passwordmail),
                    EnableSsl = true
                };

                var maillmessage = new MailMessage
                {
                    From = new MailAddress(_gmail),
                    Subject = "Ваш файл уже готов!",
                    Body = $"Файл: {System.IO.Path.GetFileName(FilePath)}\n\nОтправлено: {DateTime.Now}",
                    IsBodyHtml = false
                };

                maillmessage.To.Add(Email);

                var attachment = new Attachment(FilePath);
                maillmessage.Attachments.Add(attachment);

                await smptclient.SendMailAsync(maillmessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение в FileSaveMail -> MailSendYandex -> smptserververifi" + ex.Message);
            }
        }
    }
}
