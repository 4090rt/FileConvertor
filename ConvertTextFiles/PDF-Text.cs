using Aspose.Pdf.Operators;
using Aspose.Pdf.Text;
using FileSaveMail;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace cONVERTPDFTEXT
{
    public class PDF_Text
    {
        private string _FilePath;
        private  long _filerazmer;
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
                    Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(FilePath);
                    pdfDocument.Save(outpath, Aspose.Pdf.SaveFormat.DocX);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось конвертировать файл cONVERTPDFTEXT -> PDF-Word -> fileconvert" + ex.Message);
                }
            }

            public async Task<string> Filesave(string FilePath)
            {
                PoolongSaveFileDialog pool = new PoolongSaveFileDialog();
                SaveFileDialog saveFileDialog = null;
                try
                {
                    saveFileDialog = pool.GetSavefiledialog();
                    saveFileDialog.Title = "Сохранение файла";
                    saveFileDialog.Filter = "Word Document (*.docx)|*.docx|Word 97-2003 (*.doc)|*.doc|Все файлы (*.*)|*.*";
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
                                return path;
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

        public async Task<string> FilePath()
        {
            PoolongSaveFileDialog pool = new PoolongSaveFileDialog();
            SaveFileDialog saveFileDialog = null;
            try
            {
                saveFileDialog = pool.GetSavefiledialog();
                saveFileDialog.Title = "Конвертация";
                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == true)
                {
                    var pathfile = saveFileDialog.FileName;
                    string pathexti = Path.GetExtension(pathfile);
                    if (pathexti.ToLower() == ".txt")
                    {

                        return pathfile;
                    }
                    else
                    {
                        return "Неверное расширение файла!";
                    }
                }
                else
                {
                    return "Выберите файл";
                }                           
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось получить путь к файлу cONVERTPDFTEXT -> PDF-Text -> File" + ex.Message);
                return "Ошибка";
            }
            finally
            {
                pool.RefreshFilegialog(saveFileDialog);
            }
        }

        public async Task<string[]> FileReading()
        {
            var filepath = _FilePath;
            if (File.Exists(filepath) && filepath != "Выберите файл" && filepath != "Неверное расширение файла!" && filepath != "Ошибка")
            {

                    FileInfo FILEINFO = new FileInfo(filepath);
                    _filerazmer = FILEINFO.Length;
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
                MessageBox.Show("файл не существует");
                return null;
            }
        }

        public async Task convertpdf(string outputPath)
        {
            string[] massiv = await FileReading();
            try
            {
                Aspose.Pdf.Document doc = new Aspose.Pdf.Document();
                Aspose.Pdf.Page page = doc.Pages.Add();

                if (massiv != null)
                {
                    foreach (var exp in massiv)
                    {
                        if (!string.IsNullOrEmpty(exp))
                        {
                            TextFragment textFragment = new TextFragment(exp);
                            textFragment.TextState.Font = FontRepository.FindFont("Arial");
                            textFragment.TextState.FontSize = 10;
                            page.Paragraphs.Add(textFragment);
                        }
                    }
                    doc.Save(outputPath);
                }
                else
                {
                    MessageBox.Show("Массив пуст!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось конвертировать файл cONVERTPDFTEXT -> PDF-Text -> convertpdf" + ex.Message);
            }
        }

        public async Task<string> FileSave(string Email,bool Filesave, bool FileEmail)
        {
            PoolongSaveFileDialog pool = new PoolongSaveFileDialog();
            SaveFileDialog saveFileDialog = null;
            try
            {
                saveFileDialog = pool.GetSavefiledialog();
                saveFileDialog.Title = "Сохранение";
                saveFileDialog.Filter = "PDF файлы (*.pdf)|*.pdf|Все файлы (*.*)|*.*";
                if (Filesave || FileEmail)
                {

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string path = saveFileDialog.FileName;
                        string disk = Path.GetPathRoot(path);
                        DriveInfo drive = new DriveInfo(disk);
                        long freeSpace = drive.AvailableFreeSpace;
                        if (_filerazmer > freeSpace)
                        {
                            return "На диске не достаточно свобожного места";
                        }

                        string pathgetex = Path.GetExtension(path);
                        if (pathgetex.ToLower() == ".pdf")
                        {
                            await convertpdf(path);
                            MessageBox.Show($"Файл успешно сохранен по пути {path}");
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
                            return "Попытка сохранить отличный от pdf файл!";
                        }
                    }
                    else
                    {
                        return "Выберите директорию для сохранения";
                    }
                }
                return "Выберите вариант сохранения";
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
