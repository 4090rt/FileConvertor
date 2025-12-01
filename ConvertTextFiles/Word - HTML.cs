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
    public class Word___HTML
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
                saveFileDialog.Filter = "Word Documents (*.docx)|*.docx|Word 97-2003 (*.doc)|*.doc|Все файлы (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string FilePath = saveFileDialog.FileName;
                    string pathh = System.IO.Path.GetExtension(FilePath);
                    if (pathh.ToLower() == ".docx")
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
        public async Task converthtml(string FilePath, string outpath)
        {
            try
            {
               FileInfo fileonfo = new FileInfo(FilePath);
                _filerazmer = fileonfo.Length;
                if (_filerazmer < 524288000)
                {
                    Aspose.Words.Document document = new Aspose.Words.Document(FilePath);
                    document.Save(outpath,(Aspose.Words.SaveFormat.Html));
                }
                else
                {
                    MessageBox.Show("Файл слишком большой для конвертации");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось конвертировать файл cONVERTPDFTEXT -> Word-HTML -> converthtml" + ex.Message);
            }
        }

        public async Task<string> savehtml(string FilePaTH)
        { 
            PoolongSaveFileDialog pool = new PoolongSaveFileDialog();
            SaveFileDialog saveFileDialog = null;
            try
            {
                saveFileDialog = pool.GetSavefiledialog();
                saveFileDialog.Title = "Сохранение файла";
                saveFileDialog.Filter = "HTML файлы (*.html)|*.html|Все файлы (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string path = saveFileDialog.FileName;
                    string pathh = Path.GetPathRoot(path);
                    DriveInfo driveinfo = new DriveInfo(pathh);
                    long freespace = driveinfo.AvailableFreeSpace;
                    if (freespace > _filerazmer * 2)
                    { 
                        string filerrach = Path.GetExtension(path);
                        if (filerrach.ToLower() == ".html" || filerrach.ToLower() == ".htm")
                        { 
                            await converthtml(FilePaTH, path).ConfigureAwait(false);
                            return path;
                        }
                        else
                        {
                            return "Попытка сохранить отличный от html файл";
                        }
                    }
                    else
                    {
                        return "На диске недостаточно места для сохранения";
                    }
                }
                else
                {
                    return ("Выберите файл для сохранения");
                }
            }
            catch (Exception ex)
            {
                return "Не удалось сохранить файл cONVERTPDFTEXT -> PDF - HTML -> Savehtml" + ex.Message;
            }
            finally
            {
                pool.RefreshFilegialog(saveFileDialog);
            }
        }
    }
}

