using Aspose.Pdf.AI;
using Aspose.Pdf.Text;
using Aspose.Words;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace cONVERTPDFTEXT
{
    public class Word___Text
    {
        private long _filerazmer;

        public async Task<string> Filepath()
        {
            PoolongSaveFileDialog pool = new PoolongSaveFileDialog();
            SaveFileDialog saveFileDialog = null;
            try
            {
                saveFileDialog = pool.GetSavefiledialog();
                saveFileDialog.Title = "Файл для импорта";
                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

                if (saveFileDialog.ShowDialog() == true)
                {
                    string FilePath = saveFileDialog.FileName;
                    string pathexti = Path.GetExtension(FilePath);
                    if (pathexti.ToLower() == ".txt")
                    {
                        return FilePath;
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


        public async Task<string[]> Filereading(string filepath)
        {
            if (File.Exists(filepath) && filepath != "Выберите файл" && filepath != "Неверное расширение файла!" && filepath != "Ошибка")
            {
                FileInfo file = new FileInfo(filepath);
                _filerazmer = file.Length;

                if (_filerazmer < 524288000)
                {
                    try
                    {
                        string[] massiv = await File.ReadAllLinesAsync(filepath).ConfigureAwait(false);
                        return massiv;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Не удалось прочитать файл cONVERTPDFTEXT -> PDF-Text -> FileReading" + ex.Message);
                        return null;
                    }
                }
                else
                {
                    MessageBox.Show("Файл слишком большой для конвертации, Ошибка!");
                    return null;
                }
            }
            else
            {
                MessageBox.Show("файл не существует или не найден");
                return null;
            }
        }

        public async Task Convertfile(string FilePath,string outpath)
        {
            string[] massiv = await Filereading(FilePath);
            try
            {
                Aspose.Words.Document doc = new Aspose.Words.Document();
                DocumentBuilder builder = new DocumentBuilder(doc);

                if (massiv != null)
                {
                    foreach (var exp in massiv)
                    {
                        if (!string.IsNullOrEmpty(exp))
                        {
                            builder.Font.Name = "Arial";
                            builder.Font.Size = 10;
                            builder.Write(exp);
                            builder.Writeln();
                        }
                    }
                    doc.Save(outpath);
                }
                else
                {
                    MessageBox.Show("Массив пуст");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось конвертировать файл cONVERTPDFTEXT -> Word-Text -> Convertfile" + ex.Message);
            }
        }

        public async Task<string> SaveFile(string FilePath)
        {
            PoolongSaveFileDialog pool = new PoolongSaveFileDialog();
            SaveFileDialog saveFileDialog = null;
            try
            {
                saveFileDialog = pool.GetSavefiledialog();
                saveFileDialog.Title = "Сохранеие";
                saveFileDialog.Filter = "Word Document (*.docx)|*.docx|Word 97-2003 (*.doc)|*.doc|Все файлы (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == true)
                {
                    var path = saveFileDialog.FileName;
                    string disk = Path.GetPathRoot(path);
                    DriveInfo drive = new DriveInfo(disk);
                    long drivelenght = drive.AvailableFreeSpace;
                    if (_filerazmer > drivelenght)
                    {
                        return "На диске нет свободного места!";
                    }
                    string pathh = Path.GetExtension(path);
                    if (pathh.ToLower() == ".docx")
                    {
                        await Convertfile(FilePath, path);
                        MessageBox.Show($"Файл успешно сохранен по пути {path}");
                        return path;
                    }
                    else
                    {
                        return "Попытка сохранить отличный от pdf файл!";
                    }
                }
                else
                {
                    return "Выберите директорию для сохранения";
                }
            }
            catch (Exception ex)
            {
                return "Не удалось сохранить файл cONVERTPDFTEXT -> PDF-Text -> FileSave" + ex.Message;
            }
            finally
            {
                pool.RefreshFilegialog(saveFileDialog);
            }
        }
    }
}

