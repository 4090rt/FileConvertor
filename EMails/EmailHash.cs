using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileSaveMail
{
    public class EmailHash
    {
        public  string HashService(string Email)
        {
            try
            {
                if (!string.IsNullOrEmpty(Email))
                {
                    using (SHA256 sHA256 = SHA256.Create())
                    {
                        byte[] bytes = sHA256.ComputeHash(Encoding.UTF8.GetBytes(Email));
                        StringBuilder BUILDER = new StringBuilder();
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            BUILDER.Append(bytes[i].ToString("x2"));
                        }
                        return BUILDER.ToString();
                    }
                }
                throw new Exception("Ошибка Получения хэша данных");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение в FileSaveEmail -> EmailHash -> HashService" + ex.Message);
                throw;
            }
        }
    }
}
