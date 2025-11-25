
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Database
{
    public class USER
    {
        public int Id { get; set; }
        public string OperationName { get; set; }
        public string TypeOperation { get; set; }
        public DateTime DateOperation { get; set; }

    }
    public interface ISqliteRepository<USER>
    {
       Task<int> KolvoZap();
       Task<List<USER>> AllZap();
       Task<List<USER>> PoNAZV(string OperationName);
       Task<string> Last();
       Task<List<USER>> POType(string TypeName);
    }

    public  class SqliteRepository: ISqliteRepository<USER>
    {
        public async Task InitializeAsync()
        {
            await indexPoNAZV();
            await indexPoNazvProverka();
            await indexLast();
            await indexLastProverka();
            await iNDEXpOTYPE();
            await iNDEXpOTYPEProvarka();
        }

        public async Task<int> KolvoZap()
            { 
                PoolHTTPConnect pool = new PoolHTTPConnect();
                SQLiteConnection connect = null;
            try
            {
                connect = pool.Connectbd();
                using (var command = new SQLiteCommand("SELECT COUNT(*) FROM Users1 ", connect))
                {
                    var commandresult = await command.ExecuteScalarAsync().ConfigureAwait(false);
                    bool result = commandresult != null;
                    MessageBox.Show(result ? $"Количество записей {commandresult}" : $"Записи не найдены");
                    return (int)commandresult;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение при подсчете записей в БД: RepositoryFilter ->  SqliteRepository<T> -> KolvoZap" + ex.Message);
                return 0;
            }
            finally
            {
                pool.disconectbd(connect);
            }
            }


            public async Task<List<USER>>  AllZap()
            {
                var operations = new List<USER>();
                PoolHTTPConnect pool = new PoolHTTPConnect();
                SQLiteConnection connect = null;
                try
                {
                    connect = pool.Connectbd();

                    using (var command = new SQLiteCommand("SELECT * FROM Users1", connect))
                    {
                        using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                        {
                            while (await reader.ReadAsync())
                            {
                                operations.Add(new USER
                                {
                                    Id = reader.GetInt32(0),
                                    OperationName = reader.GetString(1),
                                    TypeOperation = reader.GetString(2),
                                    DateOperation = reader.GetDateTime(3)                                
                                });
                            }
                        MessageBox.Show($"{operations}");
                        }
                    }
                    return operations;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Возникло исключение при отображении записей в БД: RepositoryFilter ->  SqliteRepository<T> -> AllZap" + ex.Message);
                    return operations;
                }
                finally
                {
                    pool.disconectbd(connect);
                }
            }

         public async Task <List<USER>> PoNAZV(string OperationName)
         {
            var operations = new List<USER>();
            PoolHTTPConnect pool = new PoolHTTPConnect();
            SQLiteConnection connection = null;
            try
            {
                connection = pool.Connectbd();

                using (var command = new SQLiteCommand(@"
                SELECT Id, OperationName, TypeOperation, DateOperation 
                FROM Users1 
                WHERE OperationName = @O", connection))
                {
                    command.Parameters.AddWithValue("@O", OperationName);
                    using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync())
                        {
                            operations.Add(new USER
                            {
                                Id = reader.GetInt32(0),
                                OperationName = reader.GetString(1),
                                TypeOperation = reader.GetString(2),
                                DateOperation = reader.GetDateTime(3)
                            });
                        }
                        MessageBox.Show($"{operations}");
                    }
                }
                return operations;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение при отображении записей в БД: RepositoryFilter ->  SqliteRepository<T> -> PoNAZV" + ex.Message);
                return operations;
            }
            finally
            {
                pool.disconectbd(connection);
            }
         }

        public async Task indexPoNAZV()
        {
                PoolHTTPConnect pool = new PoolHTTPConnect();
                SQLiteConnection connection = null;
            try
            {
                connection = pool.Connectbd();

                using (var command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS IX_Users1_OperationName ON Users1(OperationName)", connection))
                {
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение при Создании индекса в БД: Repositoryfilter -> SqliteRepository -> indexPoNAZV" + ex.Message);
            }
            finally
            {
                pool.disconectbd(connection);
            }
        }

        public async Task indexPoNazvProverka() 
        {
            PoolHTTPConnect pool = new PoolHTTPConnect();
            SQLiteConnection connection = null;
            try
            {
                connection = pool.Connectbd();

                using (var command = new SQLiteCommand("SELECT 1 FROM sqlite_master WHERE type = 'index' AND name = 'IX_Users1_OperationName'", connection))
                {
                    var resultcommand = await command.ExecuteScalarAsync().ConfigureAwait(false);
                    bool result = resultcommand != null;
                    MessageBox.Show(result ? $"✅ Индекс '{result.ToString()}' создан успешно!" : "❌ Индекс не создан");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Возникло исключение при Проверке индекса в БД:  Repositoryfilter -> SqliteRepository -> indexPoNAZVProverka" + ex.Message);
            }
            finally
            {
                pool.disconectbd(connection);
            }
        }


         public async Task<string> Last()
         {
            PoolHTTPConnect pool = new PoolHTTPConnect();
            SQLiteConnection connection = null;
            try
            {
                connection = pool.Connectbd();

                using (var command = new SQLiteCommand("SELECT Id, OperationName, TypeOperation," +
                    " DateOperation FROM Users1" +
                    " ORDER BY DateOperation ASC LIMIT 1", connection))
                {
                    var commandresult = await command.ExecuteScalarAsync().ConfigureAwait(false);
                    bool result = commandresult != null;
                    MessageBox.Show(result ? $"Последняя запись {commandresult}" : $"Записи не найдены");
                    return commandresult.ToString();
                }               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение при отображении записей в БД: RepositoryFilter ->  SqliteRepository<T> -> Last" + ex.Message);
                return "";
            }
            finally
            {
                pool.disconectbd(connection);
            }
         }



        public async Task indexLast()
        {
            PoolHTTPConnect pool = new PoolHTTPConnect();
            SQLiteConnection connection = null;
            try
            {
                connection = pool.Connectbd();

                using (var command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS IX_Users1_DateOperation ON Users1(DateOperation)", connection))
                {
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение при Создании индекса в БД: Repositoryfilter -> SqliteRepository -> indexLast" + ex.Message);
            }
            finally
            {
                pool.disconectbd(connection);
            }
        }

        public async Task indexLastProverka()
        {
            PoolHTTPConnect pool = new PoolHTTPConnect();
            SQLiteConnection connection = null;
            try
            {
                connection = pool.Connectbd();

                using (var command = new SQLiteCommand("SELECT 1 FROM sqlite_master WHERE type = 'index' AND name = 'IX_Users1_DateOperation'", connection))
                {
                    var resultcommand = await command.ExecuteScalarAsync().ConfigureAwait(false);
                    bool result = resultcommand != null;
                    MessageBox.Show(result ? $"✅ Индекс '{result.ToString()}' создан успешно!" : "❌ Индекс не создан");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Возникло исключение при Проверке индекса в БД:  Repositoryfilter -> SqliteRepository -> indexLastProverka" + ex.Message);
            }
            finally
            {
                pool.disconectbd(connection);
            }
        }

        public async Task<List<USER>> POType(string TypeName)
        {
            PoolHTTPConnect pool = new PoolHTTPConnect();
            SQLiteConnection connect = null;
            var operations = new List<USER>();
            try
            {
                connect = pool.Connectbd();
                using (var command = new SQLiteCommand(@"
                SELECT Id, OperationName, TypeOperation, DateOperation 
                FROM Users1 
                WHERE TypeOperation = @O", connect))
                {
                    command.Parameters.AddWithValue("@O", TypeName);
                    using (var result = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await result.ReadAsync())
                        {
                            operations.Add(new USER
                            {
                                Id = result.GetInt32(0),
                                OperationName = result.GetString(1),
                                TypeOperation = result.GetString(2),
                                DateOperation = result.GetDateTime(3)
                            });
                        }
                        MessageBox.Show($"{operations}");
                    }
                }
                return operations;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Возникло исключение при Проверке индекса в БД:  Repositoryfilter -> SqliteRepository -> POType" + ex.Message);
                return operations;
            }
            finally
            {
                pool.disconectbd(connect);
            }
        }

        public async Task iNDEXpOTYPE()
        {
            PoolHTTPConnect pool = new PoolHTTPConnect();
            SQLiteConnection connect = null;
            try
            {
                connect = pool.Connectbd();
                using (var command = new SQLiteCommand("CREATE INDEX IF NOT EXISTS IX_Users1_TypeOperation ON Users1(TypeOperation)"))
                {
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение при Создании индекса в БД: Repositoryfilter -> SqliteRepository -> iNDEXpOTYPE" + ex.Message);
            }
            finally
            {
                pool.disconectbd(connect);
            }
        }

        public async Task iNDEXpOTYPEProvarka()
        {
            PoolHTTPConnect pool = new PoolHTTPConnect();
            SQLiteConnection connect = null;
            try 
            {
                connect = pool.Connectbd();
                using (var command = new SQLiteCommand("SELECT 1 FROM sqlite_master WHERE type = 'index' AND name = 'IX_Users1_DateOperation'", connect))
                {
                  var resultcommand = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                  bool result = resultcommand != null;
                  MessageBox.Show(result ? $"✅ Индекс '{result.ToString()}' создан успешно!" : "❌ Индекс не создан");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение при Создании индекса в БД: Repositoryfilter -> SqliteRepository -> iNDEXpOTYPEProvarka" + ex.Message);
            }
            finally
            {
                pool.disconectbd(connect);
            }
        }
    }
}
