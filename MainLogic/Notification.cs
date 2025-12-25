using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace WpfApp1
{
    public class Notification
    {
        public void Notif()
        {
            var notification = new NotifyIcon()
            {
                Icon = SystemIcons.Application,
                Visible = true,
                Text = "Конвертация"
            };

            notification.ShowBalloonTip(4000, "Конвертация Зевершена!", "Можете продолжить работу с конвертированными файлами", ToolTipIcon.Info);
            
            autodispose(notification);
        }

        public void Notifnot()
        {
            var notification = new NotifyIcon()
            {
                Icon  = SystemIcons.Application,
                Visible = true,
                Text = "Конвертация"
            };

            notification.ShowBalloonTip(4000, "Неудача!", "Неудалось конвертировать файл", ToolTipIcon.Info);

            autodispose(notification);
        }

        public void NotifEmail()
        {
            var notification = new NotifyIcon()
            {
                Icon = SystemIcons.Application,
                Visible = true,
                Text = "Конвертация"
            };
            notification.ShowBalloonTip(4000, "Конвертация Зевершена!", "Файл отправлен на почту и сохранен на диск. Можете продолжить работу с конвертированными файлами", ToolTipIcon.Info);

            autodispose(notification);
        }

        public void NotifnotEmail()
        {
            var notification = new NotifyIcon()
            {
                Icon = SystemIcons.Application,
                Visible = true,
                Text = "Конвертация"
            };

            notification.ShowBalloonTip(4000, "Неудача!", "Неудалось отправить файл на Mail. Файл сохранен по указанному пути", ToolTipIcon.Info);

            autodispose(notification);
        }

        public void NotifnotEmainotl()
        {
            var notification = new NotifyIcon()
            {
                Icon = SystemIcons.Application,
                Visible = true,
                Text = "Конвертация"
            };

            notification.ShowBalloonTip(4000, "Неудача!", "Ошибка при сохранении/отправке", ToolTipIcon.Info);

            autodispose(notification);
        }

        public void NotifnotEmailnull()
        {
            var notification = new NotifyIcon()
            {
                Icon = SystemIcons.Application,
                Visible = true,
                Text = "Конвертация"
            };

            notification.ShowBalloonTip(4000, "Неудача!", "Укажите Email", ToolTipIcon.Info);

            autodispose(notification);
        }

        public void Notifotmenal()
        {
            var notification = new NotifyIcon()
            {
                Icon = SystemIcons.Application,
                Visible = true,
                Text = "Конвертация"
            };

            notification.ShowBalloonTip(4000, "Неудача!", "Операция отменена", ToolTipIcon.Info);

            autodispose(notification);
        }
        public void autodispose(NotifyIcon notificaton)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 5;
            timer.Elapsed += (e, s) =>
            {
                notificaton.Dispose();
                timer.Dispose();
            };
            timer.Start();
        }
    }
}
