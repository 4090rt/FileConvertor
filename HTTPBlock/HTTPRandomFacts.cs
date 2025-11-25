using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace HTTPBlock
{

    public class Fact
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class HTTPRandomFacts
    {
        string URL = "https://tech-news-api.vercel.app/latest";
        object lockObj = new object();

        public async Task RandomnewsHttp()
        {
            MessageBox.Show("sddf");
            PoolHttp pool = new PoolHttp();
            HttpClient client = null;
            var resultsss = new List<object>();
            try
            {
                using (client = await pool.PoolConnect())
                {
                        HttpResponseMessage recpon = await client.GetAsync(URL);
                        recpon.EnsureSuccessStatusCode();
                        MessageBox.Show("sddf");

                        if (recpon.IsSuccessStatusCode)
                        {
                            var resulthttp = await recpon.Content.ReadAsStringAsync();
                            var resultserialis = JsonSerializer.Deserialize<List<Fact>>(resulthttp);
                            MessageBox.Show(resulthttp);
                            Type type = resulthttp.GetType();
                            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                            Parallel.ForEach(properties, prop =>
                            {
                                var value = prop.GetValue(resulthttp);
                                lock (lockObj)
                                {
                                    resultsss.Add(value);
                                }
                            });
                        }
                }
                MessageBox.Show($"{resultsss}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка Http запрос в  HTTPBlock -> HTTPRandomFacts -> HTTPRandomFacts -> RandomnewsHttp" + ex.Message);
            }
            finally
            {
                pool.PoolDisConnect(client);
            }
        }
    }
}
