using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Database
{



    public class DBCommandClass
    {
        DBPathClass DBPath = new DBPathClass();
        PoolHTTPConnect pool = new PoolHTTPConnect();
        SQLiteConnection connect = null;
        public async Task Createdatabase()
        {
            string dbpath = DBPath.dbPath();
            if (!string.IsNullOrEmpty(dbpath))
            {
                MessageBox.Show($"Строка подключения {dbpath}");
            }
            try
            {
                connect = pool.poolconnect();
                using (var SQLiteCommand = new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS Users1 (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    OperationName TEXT NOT NULL,     
                    TypeOperation TEXT NOT NULL,
                    DateOperation DATETIME DEFAULT CURRENT_TIMESTAMP
                    )", connect))
                {
                    await SQLiteCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение при создании БД: DBCommandClass -> Createdatabase" + ex.Message);
            }
            finally
            {
                pool.disconectbd(connect);
            }
        }
    }

    public class Savezap
    {
        public async Task Command(string OperationName, string TypeOperation, DateTime DateOperation)
        {
            DBPathClass DBPath = new DBPathClass();
            PoolHTTPConnect pool = new PoolHTTPConnect();
            SQLiteConnection connect = null;

            try
            {
                connect = pool.Connectbd();

                using (var command = new SQLiteCommand("INSERT INTO [Users1] (OperationName, TypeOperation, DateOperation) VALUES (@O, @T, @D)", connect))
                {
                    command.Parameters.AddWithValue("@O", OperationName);
                    command.Parameters.AddWithValue("@T", TypeOperation);
                    command.Parameters.AddWithValue("D", DateOperation);
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                    MessageBox.Show("Успешно добавлено!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение при сохранении в БД: DBCommandClass -> Savezap" + ex.Message);
            }
            finally
            {
                pool.disconectbd(connect);
            }
        }
    }
    public class UserOperation
    {
        public int Id { get; set; }
        public string OperationName { get; set; }
        public string TypeOperation { get; set; }
        public DateTime DateOperation { get; set; }

    }

    public class OrderBycommand 
    {
        public async Task InitializeAsync()
        {
            await Index();
            await Indexproverka();
        }

        public async Task<List<UserOperation>> Command()
            {
            var operations = new List<UserOperation>();
            DBPathClass DBPath = new DBPathClass();
            PoolHTTPConnect pool = new PoolHTTPConnect();
            SQLiteConnection connect = null;

            try
            {
                connect = pool.Connectbd();

                using (var command = new SQLiteCommand(@"
                SELECT Id, OperationName, TypeOperation, DateOperation 
                FROM Users1 
                ORDER BY DateOperation DESC 
                LIMIT 5", connect))
                {
                    using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync())
                        {
                            operations.Add(new UserOperation
                            {
                                Id = reader.GetInt32(0),
                                OperationName = reader.GetString(1),
                                TypeOperation = reader.GetString(2),
                                DateOperation = reader.GetDateTime(3)
                            });
                        }
                    }
                }
                return operations;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выборке: " + ex.Message);
                return operations;
            }
            finally
            {
                pool.disconectbd(connect);
            }
        }


        public async Task Index()
        {
            DBPathClass DBPath = new DBPathClass();
            PoolHTTPConnect pool = new PoolHTTPConnect();
            SQLiteConnection connect = null;
            try
            {
                connect = pool.Connectbd();

                using (var command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS IX_Users1_DateOperation ON Users1(DateOperation DESC)", connect))
                {
                   await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение при Создании индекса в БД: DBCommandClass -> OrderBycommand -> Index" + ex.Message);
            }
            finally
            {
                pool.disconectbd(connect);
            }
        }


        public async Task Indexproverka()
        {
            DBPathClass DBPath = new DBPathClass();
            PoolHTTPConnect pool = new PoolHTTPConnect();
            SQLiteConnection connect = null;
            try
            {
                connect = pool.Connectbd();

                using (var command = new SQLiteCommand("SELECT 1 FROM sqlite_master WHERE type = 'index' AND name = 'IX_Users1_DateOperation'", connect))
                {
                    var commandd = await command.ExecuteScalarAsync().ConfigureAwait(false);
                    bool result = commandd != null;

                    MessageBox.Show(result ? $"✅ Индекс '{result.ToString()}' создан успешно!" : "❌ Индекс не создан");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение при Проверке индекса в БД: DBCommandClass -> OrderBycommand -> Indexproverka" + ex.Message);
            }
            finally
            {
                pool.disconectbd(connect);
            }
        }
    }
}
