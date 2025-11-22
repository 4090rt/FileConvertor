using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Database
{
    public class DBPathClass
    {
        public string dbPath()
        {
            try
            {
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                StringBuilder debugInfo = new StringBuilder();
                debugInfo.AppendLine($"Documents path: '{documentsPath}'");
                debugInfo.AppendLine($"Path exists: {Directory.Exists(documentsPath)}");

                string appFolder = Path.Combine(documentsPath, "WinFormsApp4");
                debugInfo.AppendLine($"App folder: '{appFolder}'");
                debugInfo.AppendLine($"App folder exists: {Directory.Exists(appFolder)}");

                // Создаем папку если нужно
                if (!Directory.Exists(appFolder))
                {
                    Directory.CreateDirectory(appFolder);
                    debugInfo.AppendLine("App folder created");
                }

                string dbPath = Path.Combine(appFolder, "UserBase.db");
                debugInfo.AppendLine($"DB path: '{dbPath}'");
                debugInfo.AppendLine($"DB file exists: {File.Exists(dbPath)}");

                // ПРОВЕРКА ПРАВ ЗАПИСИ
                try
                {
                    string testFile = Path.Combine(appFolder, "test.tmp");
                    File.WriteAllText(testFile, "test");
                    File.Delete(testFile);
                    debugInfo.AppendLine("Write access: OK");
                }
                catch (Exception ex)
                {
                    debugInfo.AppendLine($"Write access FAILED: {ex.Message}");
                }

                MessageBox.Show(debugInfo.ToString(), "Диагностика пути БД");
                return dbPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка в GetDatabasePath: {ex.Message}");
                // Резервный путь
                return Path.Combine(Path.GetTempPath(), "UserBase.db");
            }
        }
    }
}
