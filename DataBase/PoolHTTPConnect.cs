using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    class PoolHTTPConnect
    {
        public string _dbpath;
        private readonly Stack<SQLiteConnection> _pool = new Stack<SQLiteConnection>();
        private readonly int _maxPoolSize = 10;
        public PoolHTTPConnect()
        {
            DBPathClass dB = new DBPathClass();
            _dbpath = dB.dbPath();
        }

        public SQLiteConnection Connectbd()
        {
            var connect = new SQLiteConnection($"Data Source={_dbpath}");
                connect.Open();
                return connect;
        }

        public SQLiteConnection poolconnect()
        {
            lock (_pool)
            {
                if (_pool.Count > 0)
                { 
                    return _pool.Pop();
                }
            }
            return Connectbd();
        }

        public void disconectbd(SQLiteConnection connection)
        {
            if (connection.State == ConnectionState.Broken || connection.State == ConnectionState.Closed)
            {
                connection.Dispose();
                return;
            }        
            lock (_pool)
            {
                if (_pool.Count < _maxPoolSize)
                {
                    _pool.Push(connection);
                }
                else
                { 
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
    }
}
