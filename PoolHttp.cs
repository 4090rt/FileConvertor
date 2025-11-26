using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HTTPBlock
{
    public class PoolHttp
    {
        public readonly Stack<HttpClient> _pool = new Stack<HttpClient>();

        public async Task<HttpClient> PoolConnect()
        {
            try
            {
                lock (_pool)
                {
                    if (_pool.Count > 0)
                    {
                        _pool.Pop();
                    }
                }
                return new  HttpClient();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка пулинга объекта HTTPClient в  HTTPBlock -> PoolHttp -> PoolConnect" + ex.Message);
                return new HttpClient();
            }
        }

        public void PoolDisConnect(HttpClient client)
        {
            try
            {
                client.DefaultRequestHeaders.Clear();
                client.CancelPendingRequests();
                lock (_pool)
                {
                    if (_pool.Count < 10)
                    {
                        _pool.Push(client);
                    }
                    else
                    {
                        client.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка пулинга объекта HTTPClient в  HTTPBlock -> PoolHttp -> PoolDisConnect" + ex.Message);
            }
        }
    }
}
