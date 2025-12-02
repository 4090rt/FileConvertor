using Aspose.Cells;
using Aspose.Pdf.AI;
using FileSaveMail;
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
    public class PDF_HTML
    {
        private long _filerazmer;
        public async Task<string> fILEPATH()
        {
            PoolongSaveFileDialog pool = new PoolongSaveFileDialog();
            SaveFileDialog savefile = null;
            try
            {
                savefile = pool.GetSavefiledialog();
                savefile.Title = "Выберите файл";
                savefile.Filter = "PDF Files (*.pdf)|*.pdf|Все файлы (*.*)|*.*";
                if (savefile.ShowDialog() == true)
                {
                    string FilePath = savefile.FileName;
                    string pathh = System.IO.Path.GetExtension(FilePath);

                    if (pathh.ToLower() == ".pdf")
                    {
                        return FilePath;
                    }
                    else
                    {
                        return "Вы выбрали файл отличный от pdf!";
                    }
                }
                else
                {
                    return "Выберите файл для конвертации";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось получить путь к файлу cONVERTPDFTEXT -> PDF-HTML -> filePATH" + ex.Message);
                return "Ошибка";
            }
            finally
            {
                pool.RefreshFilegialog(savefile);
            }
        }


        public async Task converttohtml(string FilePath, string outpath)
        {
            try
            {
                FileInfo fileinfo = new FileInfo(FilePath);
                _filerazmer = fileinfo.Length;
                if (_filerazmer < 524288000)
                {
                    Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(FilePath);
                    pdfDocument.Save(outpath, Aspose.Pdf.SaveFormat.Html);
                }
                else
                {
                    MessageBox.Show("Файл слишком большой для конвертации");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось конвертировать файл cONVERTPDFTEXT -> PDF-HTML -> converttohtml" + ex.Message);
            }
        }


        public async Task<string> Savehtml(string FilePath, bool FileEmail, bool Filesave, string Email)
        { 
            PoolongSaveFileDialog pool = new PoolongSaveFileDialog();
            SaveFileDialog savedialog = null;
            try
            {
                savedialog = pool.GetSavefiledialog();
                savedialog.Title = "Сохранение файла";
                savedialog.Filter = "HTML файлы (*.html)|*.html|Все файлы (*.*)|*.*";
                if (FileEmail || Filesave)
                {
                    if (savedialog.ShowDialog() == true)
                    {
                        string path = savedialog.FileName;
                        string pathh = System.IO.Path.GetPathRoot(path);
                        DriveInfo DRIVE = new DriveInfo(pathh);
                        long freespace = DRIVE.AvailableFreeSpace;
                        if (freespace > _filerazmer * 2)
                        {
                            string rach = System.IO.Path.GetExtension(path);
                            if (rach.ToLower() == ".html" || rach.ToLower() == ".htm")
                            {
                                await converttohtml(FilePath, path).ConfigureAwait(false);
                                if (FileEmail == true && !string.IsNullOrEmpty(Email))
                                {
                                    FileInfo fileinfo = new FileInfo(path);
                                    long razmer = fileinfo.Length;
                                    if (razmer < 26214400)
                                    {
                                        MailSend mail = new MailSend();
                                        mail.smptserververifi(Email, path);
                                    }
                                    return path;
                                }
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
                return "Выберите вариант сохранения";
            }
            catch (Exception ex)
            {
                return "Не удалось сохранить файл cONVERTPDFTEXT -> PDF - HTML -> Savehtml" + ex.Message;
            }
            finally
            {
                pool.RefreshFilegialog(savedialog);
            }
        }
    }
}
