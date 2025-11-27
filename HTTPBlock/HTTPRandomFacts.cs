using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace HTTPBlock
{

    public class AdviceResponse
    {
        [JsonPropertyName("slip")]
        public Slip Slip { get; set; }
    }

    public class Slip
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("advice")]
        public string Advice { get; set; }
    }
    public class IP
    {
        [JsonPropertyName("ip")]
        public string ip { get; set; }

        [JsonPropertyName("hostname")]
        public string hostname { get; set; }

        [JsonPropertyName("city")]
        public string city{ get; set; }

        [JsonPropertyName("region")]
        public string region { get; set; }

        [JsonPropertyName("country")]
        public string country { get; set; }

        [JsonPropertyName("loc")]
        public string loc { get; set; }

        [JsonPropertyName("org")]
        public string org { get; set; }

        [JsonPropertyName("postal")]
        public string postal { get; set; }

        [JsonPropertyName("timezone")]
        public string timezone { get; set; }
    }

    public class HTTPRandomFacts1
    {
        string URL = "https://api.adviceslip.com/advice";
        object lockObj = new object();

        public async Task RandomnewsHttp1()
        {
            PoolHttp pool = new PoolHttp();
            HttpClient client = null;
            try
            {
                client = await pool.PoolConnect();
                HttpResponseMessage recpon = await client.GetAsync(URL).ConfigureAwait(false);
                recpon.EnsureSuccessStatusCode();
                if (recpon.IsSuccessStatusCode)
                {
                    var resulthttp = await recpon.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var resultserialis = JsonSerializer.Deserialize<AdviceResponse>(resulthttp);
                    Type type = resultserialis.GetType();
                    var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    MessageBox.Show(resulthttp);
                    string resultText = "";
                    Parallel.ForEach(properties, prop =>
                    {
                        var value = prop.GetValue(resultserialis);
                        lock (lockObj)
                        {
                            resultText += $"{prop.Name}: ";
                        }
                    });
                    MessageBox.Show($"{resultText}");
                    MessageBox.Show($" {resultserialis.Slip.Id}, {resultserialis.Slip.Advice}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка Http запрос в  HTTPBlock -> HTTPRandomFacts -> HTTPRandomFacts -> RandomnewsHttp1" + ex.Message);
            }
            finally
            {
                pool.PoolDisConnect(client);
            }
        }
    }


    public class HTTPRandomFacts2
    {
        string Url = "https://ipinfo.io/json";
        private readonly object _lock = new object();

        public async Task RandomnewsHttp2()
        {
            PoolHttp pool = new PoolHttp();
            HttpClient client = null;
            try
            {
                client = await pool.PoolConnect();

                HttpResponseMessage recpon = await client.GetAsync(Url).ConfigureAwait(false);
                recpon.EnsureSuccessStatusCode();
                if (recpon.IsSuccessStatusCode)
                { 
                    var result = await recpon.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var resultserialis = JsonSerializer.Deserialize<IP>(result);
                    Type type = resultserialis.GetType();
                    var prop = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    string resultText = "";
                    Parallel.ForEach(prop, prop =>
                    {
                        var value = prop.GetValue(resultserialis);
                        lock (_lock)
                        {
                             resultText += $"{prop.Name}: {value} ";
                        }
                    });
                    if (resultText != null)
                    {
                        MessageBox.Show("Cвойства" + resultText);
                    }
                    else
                    {
                        MessageBox.Show("Свойств не найдено");
                    }
                    if (resultserialis != null)
                    {
                        MessageBox.Show($"IP:{resultserialis.ip}\nHostname:{resultserialis.hostname}\n                   City:{resultserialis.city}\n" +
                        $"Region:{resultserialis.region}\nCountry:{resultserialis.country}\nLoc: {resultserialis.loc}\n" +
                        $"Org:{resultserialis.org}\nPostal:{resultserialis.postal}\nTimezone:{resultserialis.timezone}\n");
                    }
                    else
                    {
                        MessageBox.Show("Результаты не найдены");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка Http запрос в  HTTPBlock -> HTTPRandomFacts -> HTTPRandomFacts -> RandomnewsHttp2" + ex.Message);
            }
            finally
            {
                pool.PoolDisConnect(client);
            }
        }
    }


}
