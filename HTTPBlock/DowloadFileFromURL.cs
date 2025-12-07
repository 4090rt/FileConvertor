using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HTTPBlock
{
    // Настройка клиента HTTP
    public class HttpCLientSettigns
    {
        PoolHttp pool = new PoolHttp();
        HttpClient httpClient = null;

        public async Task<HttpClient> Client()
        {
            try
            {
                httpClient = await  pool.PoolConnect();

                httpClient.Timeout = TimeSpan.FromSeconds(300);

                HttpClientHeaders(httpClient);
                return httpClient;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                pool.PoolDisConnect(httpClient);
            }
        }

        public void HttpClientHeaders(HttpClient client)
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) FileConverter/1.0");
            client.DefaultRequestHeaders.Accept.ParseAdd("*/*");
            client.DefaultRequestHeaders.Date = DateTime.Now;
        }
    }

    //Получаем имя файла 2 путями, либо с помощью заголовка, либо с помощью uri из URL
    public class FilenameHelper()
    {
        public static async Task<string> GetFileNameFromResponse(HttpResponseMessage response, string url)
        {
            if (response.Content.Headers.ContentDisposition != null)
            { 
                string name = response.Content.Headers.ContentDisposition.FileName;
                if (!string.IsNullOrEmpty(name))
                { 
                    return name.Trim('"', '\'');
                }
            }
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                string name = Path.GetFileName(uri.LocalPath);
                if (!string.IsNullOrEmpty(name))
                {
                    return name;
                }
            }
            return "downloaded_file";
        }
    }
    

    //Получаем тип файла при загрузке и присваиваем ему расширение
    public class FuleExtastation()
    {
        public static string GetExtition(string ContentType)
        {
            if (string.IsNullOrEmpty(ContentType))
            {
                return ".tmp";
            }

            string type = ContentType.ToLower();

            if (type.Contains("pdf"))
                return ".pdf";
            else if (type.Contains("msword"))
                return ".doc";
            else if (type.Contains("wordprocessingml"))
                return ".docx";
            else if (type.Contains("excel") || type.Contains("spreadsheetml"))
                return ".xlsx";
            else if (type.Contains("jpeg"))
                return ".jpg";
            else if (type.Contains("png"))
                return ".png";
            else if (type.Contains("gif"))
                return ".gif";
            else if (type.Contains("text"))
                return ".txt";
            else if (type.Contains("html"))
                return ".html";
            else if (type.Contains("zip"))
                return ".zip";
            else if (type.Contains("rar"))
                return ".rar";
            else
                return ".tmp";
        }
    }
}
