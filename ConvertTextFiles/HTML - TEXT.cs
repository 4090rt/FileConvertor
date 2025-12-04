using Aspose.Cells;
using Aspose.Pdf.Operators;
using FileSaveMail;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace cONVERTPDFTEXT
{
    public class HTML___TEXT
    {
        private long _filerazmer;
        private CancellationTokenSource _ctg;
        public string Filepath(CancellationToken cancellationToken = default)
        {
            PoollongOpenFileDialog pool = new PoollongOpenFileDialog();
            OpenFileDialog openFileDialog = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                openFileDialog = pool.PollOpen();
                openFileDialog.Title = "Выбор файла";
                openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                var result = openFileDialog.ShowDialog();
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (result == true)
                    {
                        string FilePath = openFileDialog.FileName;
                        string pathh = System.IO.Path.GetExtension(FilePath);
                        if (pathh.ToLower() == ".txt")
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
            }
            catch (OperationCanceledException)
            {
                return "Операция отменена!";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось получить путь к файлу cONVERTPDFTEXT -> Word-Text -> Fipepath" + ex.Message);
                return "Ошибка";
            }
            finally
            {
                pool.PoolClosed(openFileDialog);
            }
        }

        public async Task<string[]> Fileread(string filepath)
        { 
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

        public async Task Fileconvert(string FilePath, string outpath)
        {
            string[] massiv = await Fileread(FilePath);
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

        public async Task<string> filesave(string FilePath, bool Filesave, bool FileEmail,string Email)
        {
            PoolongSaveFileDialog pool = new PoolongSaveFileDialog();
            SaveFileDialog saveFileDialog = null;
            try
            {
                saveFileDialog = pool.GetSavefiledialog();
                saveFileDialog.Title = "Cохранение";
                saveFileDialog.Filter = "HTML Document (*.html)|*.html|Все файлы (*.*)|*.*";

                if (Filesave ||FileEmail)
                {
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
                            await Fileconvert(FilePath, path);

                            if (FileEmail == true && !string.IsNullOrEmpty(Email))
                            {
                                FileInfo fileinfo = new FileInfo(path);
                                long razmer = fileinfo.Length;
                                if (razmer < 26214400)
                                {
                                    try
                                    {
                                        Regex r1 = new Regex("@gmail.com", RegexOptions.IgnoreCase);
                                        Regex r2 = new Regex("@yandex.ru", RegexOptions.IgnoreCase);
                                        Regex r3 = new Regex("@mail.ru", RegexOptions.IgnoreCase);
                                        bool regex1 = r1.IsMatch(Email);
                                        bool regex2 = r2.IsMatch(Email);
                                        bool regex3 = r3.IsMatch(Email);
                                        if (regex1 == true)
                                        {
                                            MailSendGmail mail = new MailSendGmail();
                                            await mail.smptserververifi(Email, path);
                                        }
                                        else if (regex2 == true)
                                        {
                                            MailSendYandex mail = new MailSendYandex();
                                            await mail.smptserververifi(Email, path);
                                        }
                                        else if (regex3 == true)
                                        {
                                            MailSendYandex mail = new MailSendYandex();
                                            await mail.smptserververifi(Email, path);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        return "Не удалось сохранить файл cONVERTPDFTEXT -> HTML-Text -> filesave" + ex.Message;
                                    }
                                }
                                return path;
                            }
                            else
                            {
                                return ($"Не удалось отправить на почту, файл сохранен по указанному пути");
                            }
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
                return "Выберите способ сохранения";
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