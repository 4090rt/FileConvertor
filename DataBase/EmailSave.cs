using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Database
{
    public class EmailSave
    {
        public async Task EmailSaveComand(string Email)
        { 
            PoolHTTPConnect pool = new PoolHTTPConnect();
            SQLiteConnection connection = null;
            try
            {
                connection = pool.Connectbd();
                string commandSQL = "INSERT INTO [Users1] (Email) VALUES (@E)";
                using (var command = new SQLiteCommand(commandSQL, connection))
                {
                    command.Parameters.AddWithValue("@E",Email);
                    MessageBox.Show("Успешно добавлено!");
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }   
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение при сохранении в БД: EmailSave -> EmailSaveComand" + ex.Message);
            }
            finally
            {
                pool.disconectbd(connection);
            }
        }


        public class UserMail()
        {
            public string Email { get; set; }
        }
        public async Task<List<UserMail>> PokazEmail()
        {
            PoolHTTPConnect pool = new PoolHTTPConnect();
            SQLiteConnection connection = null;
            var Mails = new List<UserMail>();

            try
            {
                connection = pool.Connectbd();
                string commandText = "SELECT Email FROM Users1";
                using (var command = new SQLiteCommand(commandText, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync())
                        {
                            Mails.Add(new UserMail
                            {
                                Email = reader.GetString(0),
                            });
                        }
                    }
                    return Mails;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выборке: " + ex.Message);
                return null;
            }
            finally
            {
                pool.disconectbd(connection);
            }
        }
    }
}
