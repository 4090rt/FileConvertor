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
    public class Word___PDF
    {
        private long _filerazmeer;
        public async Task<string> Filepath()
        {
            PoolongSaveFileDialog pool = new PoolongSaveFileDialog();
            SaveFileDialog savefile = null;
            try
            {
                savefile = pool.GetSavefiledialog();
                savefile.Title = "Выбор файла";
                savefile.Filter = "Word Documents (*.docx)|*.docx|Все файлы (*.*)|*.*";
                if (savefile.ShowDialog() == true)
                {
                    string path = savefile.FileName;
                    string pathh = System.IO.Path.GetExtension(path);
                    if (pathh.ToLower() == ".docx")
                    {
                        return path;
                    }
                    else
                    {
                        return "Вы выбрали файл отличный от docx!";
                    }
                }
                else
                {
                    return "Выберите файл для конвертации";

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Не удалось получить путь к файлу cONVERTPDFTEXT -> WORD-PDF -> Filepath" + ex.Message);
                return "Ошибка";
            }
            finally
            {
                pool.RefreshFilegialog(savefile);
            }
        }

        public async Task fileconvert(string Filepath, string outpath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(Filepath);
                _filerazmeer = fileInfo.Length;
                if (_filerazmeer < 524288000)
                {
                    Aspose.Words.Document document = new Aspose.Words.Document(Filepath);
                    document.Save(outpath, Aspose.Words.SaveFormat.Pdf);
                }
                else 
                {
                    MessageBox.Show("Файл слишком большой для конвертации");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Не удалось конвертировать файл cONVERTPDFTEXT -> Word-PDF -> fileconvert" + ex.Message);
            }
        }

        public async Task<string> filesave(string Filepath)
        {
            PoolongSaveFileDialog pool= new PoolongSaveFileDialog();
            SaveFileDialog savefile2 = null;
            try
            {
                savefile2 = pool.GetSavefiledialog();
                savefile2.Title = "Сохранение";
                savefile2.Filter = "PDF файлы (*.pdf)|*.pdf|Все файлы (*.*)|*.*";
                if (savefile2.ShowDialog() == true)
                {
                    var path = savefile2.FileName;
                    var pathh = System.IO.Path.GetPathRoot(path);
                    DriveInfo DRIVE = new DriveInfo(pathh);
                    var freespase = DRIVE.TotalFreeSpace;
                    if (_filerazmeer < freespase)
                    {
                        string filerach = System.IO.Path.GetExtension(path);
                        if (filerach.ToLower() == ".pdf")
                        {
                            await fileconvert(Filepath, path);
                            return path;
                        }
                        else
                        {
                            return "Вы выбрали файл отличный от pdf!";
                        }
                    }
                    else
                    {
                        return "На диске не достаточно места";
                    }

                }
                else
                {
                    return "Выберите директорию для сохранения";
                }
            }
            catch (Exception ex)
            {
                return "Не удалось сохранить файл cONVERTPDFTEXT -> Word-PDF -> Filesave" + ex.Message;
            }
            finally
            {
                pool.RefreshFilegialog(savefile2);
            }
        }
    }
}
