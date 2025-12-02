using Aspose.Pdf.Text;
using FileSaveMail;
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
       public class PDFWord
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
                        string pathh = Path.GetExtension(FilePath);

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
                    MessageBox.Show("Не удалось получить путь к файлу cONVERTPDFTEXT -> PDF-WORD -> filePATH" + ex.Message);
                    return "Ошибка";
                }
                finally
                { 
                    pool.RefreshFilegialog(savefile);
                }
            }

            public async Task fileconvert(string FilePath, string outpath)
            {
                    try
                    {
                        FileInfo fileInfo = new FileInfo(FilePath);
                        _filerazmer = fileInfo.Length;
                        if (_filerazmer < 524288000)
                        {
                            Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(FilePath);
                            pdfDocument.Save(outpath, Aspose.Pdf.SaveFormat.DocX);
                        }
                        else
                        {
                            MessageBox.Show("Файл слишком большой для конвертации");
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Не удалось конвертировать файл cONVERTPDFTEXT -> PDF-Word -> fileconvert" + ex.Message);
                    }
            }

            public async Task<string> Filesave(string FilePath, string Email,bool Filesave, bool FileEmail)
            {
                PoolongSaveFileDialog pool = new PoolongSaveFileDialog();
                SaveFileDialog saveFileDialog = null;
            try
            {
                saveFileDialog = pool.GetSavefiledialog();
                saveFileDialog.Title = "Сохранение файла";
                saveFileDialog.Filter = "Word Document (*.docx)|*.docx|Word 97-2003 (*.doc)|*.doc|Все файлы (*.*)|*.*";
                if (Filesave || FileEmail)
                {
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        var path = saveFileDialog.FileName;
                        var patth = Path.GetPathRoot(path);
                        DriveInfo drive = new DriveInfo(patth);
                        long razmerdrivenesto = drive.AvailableFreeSpace;
                        if (razmerdrivenesto > _filerazmer)
                        {
                            string filerasch = Path.GetExtension(path);
                            if (filerasch.ToLower() == ".docx")
                            {
                                await fileconvert(FilePath, path);
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
                                return "Попытка сохранить файл с расширениием отличным от .docx";
                            }
                        }
                        else
                        {
                            return "На диске недостаточно места!";
                        }
                    }
                    else
                    {
                        return ("Выберите директорию для сохранения");
                    }
                }
                return "Выберите вариант сохранения";
            }
            catch (Exception ex)
            {
                return "Не удалось сохранить файл cONVERTPDFTEXT -> PDF - Word -> Filesave" + ex.Message;
            }
            finally
            {
                pool.RefreshFilegialog(saveFileDialog);
            }
            }
       }
}
