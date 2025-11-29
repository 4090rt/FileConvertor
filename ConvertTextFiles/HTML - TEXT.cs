using Aspose.Cells;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace cONVERTPDFTEXT
{
    public class HTML___TEXT
    {
        private long _filerazmer;

        public async Task<string> Filepath()
        {
            PoolongSaveFileDialog pool = new PoolongSaveFileDialog();
            SaveFileDialog saveFileDialog = null;
            try
            {
                saveFileDialog = pool.GetSavefiledialog();
                saveFileDialog.Title = "Выбор файла";
                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string path = saveFileDialog.FileName;
                    string pathh = System.IO.Path.GetExtension(path);
                    if (pathh.ToLower() == ".txt")
                    {
                        return path;
                    }
                    else
                    {
                        return "Неверное расширение файла!";
                    }
                }
                else
                {
                    return "Файл не выбран!";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось получить путь к файлу cONVERTPDFTEXT -> Word-Text -> Fipepath" + ex.Message);
                return "Ошибка";
            }
            finally
            {
                pool.RefreshFilegialog(saveFileDialog);
            }
        }

        public async Task<string[]> Fileread()
        { 
            var filepath = await Filepath();
            if ((File.Exists(filepath) && filepath != "Выберите файл" && filepath != "Неверное расширение файла!" && filepath != "Ошибка"))
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(filepath);
                    _filerazmer = fileInfo.Length;
                    if (_filerazmer < 524288000)
                    {
                        string[] massin = await File.ReadAllLinesAsync(filepath).ConfigureAwait(false);
                        return massin;
                    }
                    else
                    {
                        MessageBox.Show("Файл слишком большой для импорта!");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось прочитать файл cONVERTPDFTEXT -> HTML-Text -> FileReading" + ex.Message);
                    return null;
                }
            }
            else
            {
                MessageBox.Show("файл не существует или не найден");
                return null;
            }
        }

        public async Task Fileconvert(string outpath)
        {
            string[] massiv = await Fileread();
            try
            {
                Workbook workbook = new Workbook();
                Worksheet worksch = workbook.Worksheets[0];
                worksch.Name = "Расходы";

                if (massiv != null)
                {
                    int row = 0;
                    foreach (var exp in massiv)
                    {
                        if (!string.IsNullOrEmpty(exp))
                        {
                            worksch.Cells[row, 0].PutValue(exp);
                            row++;
                        }
                    }
                    workbook.Save(outpath);
                }
                else
                {
                    MessageBox.Show("Массив пустой!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось конвертировать файл cONVERTPDFTEXT -> HTML-Text -> Fileconvert" + ex.Message);
            }
        }

        public async Task<string> filesave()
        {
            PoolongSaveFileDialog pool = new PoolongSaveFileDialog();
            SaveFileDialog saveFileDialog = null;
            try
            {
                saveFileDialog = pool.GetSavefiledialog();
                saveFileDialog.Title = "Cохранение";
                saveFileDialog.Filter = "HTML Document (*.html)|*.html|Все файлы (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == true)
                {
                    var path = saveFileDialog.FileName;
                    var disk = System.IO.Path.GetPathRoot(path);
                    DriveInfo drive = new DriveInfo(disk);
                    var resultdrive = drive.AvailableFreeSpace;
                    if (_filerazmer > resultdrive)
                    {
                        return "На диске нет свободного места!";
                    }
                    var pathh = System.IO.Path.GetExtension(path);
                    if (pathh.ToLower() == ".html")
                    {
                        await Fileconvert(path);
                        return path;
                    }
                    else
                    {
                        return "Попытка сохранить отличный от html файл!";
                    }
                }
                else
                {
                    return "Выберите директорию для сохранения";
                }
            }
            catch (Exception ex)
            {
                return "Не удалось сохранить файл cONVERTPDFTEXT -> HTML-Text -> filesave" + ex.Message;
            }
            finally
            {
                pool.RefreshFilegialog(saveFileDialog);
            }
        }
    }
}