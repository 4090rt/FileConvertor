using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HTTPBlock
{
    public class MainFilDowloadClass
    {
        private HttpClient _httpClient;
        public async Task Initialized()
        { 
           HttpCLientSettigns client = new HttpCLientSettigns();
            _httpClient = await client.Client();
        }

        private async Task<IndormationFile> GetFileInfoAsync(HttpClient client, string url)
        {
            try
            {
                using var recpon = new HttpRequestMessage(HttpMethod.Head, url);

                var resppnse = await client.SendAsync(recpon);
                if (!resppnse.IsSuccessStatusCode)
                {
                    return null;
                }

                string Filename = await FilenameHelper.GetFileNameFromResponse(resppnse, url);

                long? Filesize = resppnse.Content.Headers.ContentLength;

                string extation = resppnse.Content.Headers.ContentType.MediaType;

                return new IndormationFile
                {
                    URL = url,
                    FileName = Filename,
                    FileSize = Filesize,
                    TargetFormat = extation
                };
            }
            catch(Exception ex)
            {
                MessageBox.Show("Исключение" + ex.Message);
                return null;
            }
        }

        private async Task<string> DownloadFileAsync(HttpClient client, string url, IndormationFile fileInfo)
        {
            try
            {
                string extetsion = FuleExtastation.GetExtition(fileInfo.TargetFormat);

                string temp = Path.GetTempPath();
                string pathname = $"{Path.GetFileNameWithoutExtension(fileInfo.FileName)}{extetsion}";
                string fullpath = Path.Combine(temp, pathname);


                fullpath = GetUniqueFilePath(fullpath);

                var recpone = await client.GetAsync(url);
                recpone.EnsureSuccessStatusCode();

                using (var stream = new FileStream(fullpath, FileMode.Create))
                {
                    await recpone.Content.CopyToAsync(stream);
                }
                return fullpath;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло Ислючение" + ex.Message);
                return null;
            }
        }

        private string GetUniqueFilePath(string filePath)
        {
            if (!File.Exists(filePath))
                return filePath;

            string directory = Path.GetDirectoryName(filePath);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);

            int counter = 1;
            string newFilePath;

            do
            {
                newFilePath = Path.Combine(directory, $"{fileNameWithoutExt} ({counter}){extension}");
                counter++;
            } while (File.Exists(newFilePath));

            return newFilePath;
        }
    }
}
