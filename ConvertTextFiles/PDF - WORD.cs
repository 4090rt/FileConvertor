using Aspose.Pdf.Text;
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
using System.Windows.Forms;
namespace cONVERTPDFTEXT
{
    public class PDFWord
    {
        private List<string> selectedFiles = new List<string>();
        private long _filerazmer;
        private CancellationTokenSource _ctg;
        public async Task<List<string>> fILEPATH(CancellationToken cancellationToken = default)
        {
            PoollongOpenFileDialog poll = new PoollongOpenFileDialog();
            OpenFileDialog savefile = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                savefile = poll.PollOpen();
                savefile.Title = "Выберите файл";
                savefile.Filter = "PDF Files (*.pdf)|*.pdf|Все файлы (*.*)|*.*";
                savefile.Multiselect = true;
                savefile.FilterIndex = 1;
                var dialog = savefile.ShowDialog();
                if (dialog == true)
                {
                    selectedFiles.Clear();
                    bool hasNonPdfFiles = false;


                    foreach (string dilepath in savefile.FileNames)
                    {
                        string pathh = Path.GetExtension(dilepath);

                        if (pathh.ToLower() == ".pdf")
                        {
                            selectedFiles.Add(dilepath);
                        }
                        else
                        {
                            hasNonPdfFiles = true;
                        }
                    }
                    if (hasNonPdfFiles)
                    {
                        MessageBox.Show("Некоторые выбранные файлы не являются PDF. Будут обработаны только PDF файлы.",
                            "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    if (selectedFiles.Count > 0)
                    {
                        return selectedFiles;
                    }
                    else
                    {
                        MessageBox.Show("Выберите хотя бы один PDF файл",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return new List<string>();
                    }
                }
                {

                    return new List<string>();
                }
            }
            catch (OperationCanceledException)
            {
                return new List<string>();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось получить путь к файлу cONVERTPDFTEXT -> PDF-WORD -> filePATH" + ex.Message);
                return new List<string>();
            }
            finally
            {
                poll.PoolClosed(savefile);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось конвертировать файл cONVERTPDFTEXT -> PDF-Word -> fileconvert" + ex.Message);
            }
        }
        public async Task<string> Filesave(List<string> selectedFiles, string Email, bool Filesave, bool FileEmail)
        {
            // Если файл один - работаем как раньше
            if (selectedFiles.Count == 1)
            {
                PoolongSaveFileDialog pool = new PoolongSaveFileDialog();
                SaveFileDialog savefile2 = null;
                try
                {
                    savefile2 = pool.GetSavefiledialog();
                    savefile2.Title = "Сохранение";
                    savefile2.Filter = "Word Document (*.docx)|*.docx";
                    if (FileEmail || Filesave)
                    {
                        if (savefile2.ShowDialog() == true)
                        {
                            var path = savefile2.FileName;
                            var pathh = System.IO.Path.GetPathRoot(path);
                            DriveInfo DRIVE = new DriveInfo(pathh);
                            var freespase = DRIVE.TotalFreeSpace;
                            if (_filerazmer < freespase)
                            {
                                string filerach = System.IO.Path.GetExtension(path);
                                if (filerach.ToLower() == ".docx")
                                {
                                    await fileconvert(selectedFiles[0], path);
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
                                                bool regex1 = r1.IsMatch(Email);
                                                bool regex2 = r2.IsMatch(Email);
                                                if (regex1 == true)
                                                {
                                                    MailSendGmail mail = new MailSendGmail();
                                                    await mail.smptserververifi(Email, path);
                                                }
                                                if (regex2 == true)
                                                {
                                                    MailSendYandex mail = new MailSendYandex();
                                                    await mail.smptserververifi(Email, path);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                return "Не удалось сохранить файл на почту cONVERTPDFTEXT -> HTML-Text -> filesave" + ex.Message;
                                            }
                                        }
                                        return path;
                                    }
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
                    return "Выберите вариант сохранения";
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
            else
            {
                PoolongSaveFileDialog pool = new PoolongSaveFileDialog();
                SaveFileDialog saveFileDialog = pool.GetSavefiledialog();

                saveFileDialog.Title = "Сохранение первого файла (остальные будут с номерами)";
                saveFileDialog.Filter = "Word Document (*.docx)|*.docx";
                saveFileDialog.FileName = "document_1.docx";

                if (saveFileDialog.ShowDialog() == true)
                {
                    string firstFilePath = saveFileDialog.FileName;
                    string folder = Path.GetDirectoryName(firstFilePath);
                    string baseName = Path.GetFileNameWithoutExtension(firstFilePath);
                    string extension = Path.GetExtension(firstFilePath);

                    int successCount = 0;

                    for (int i = 0; i < selectedFiles.Count; i++)
                    {
                        string outputPath;

                        if (i == 0)
                        {
                            outputPath = firstFilePath;
                        }
                        else
                        {
                            // Остальные файлы - добавляем номер
                            outputPath = Path.Combine(folder, $"{baseName}_{i + 1}{extension}");
                        }

                        await fileconvert(selectedFiles[i], outputPath);
                        successCount++;
                    }

                    pool.RefreshFilegialog(saveFileDialog);
                    return $"Сохранено {successCount} файлов в папку: {folder}";
                }

                pool.RefreshFilegialog(saveFileDialog);
                return "Отменено";
            }
        }
    }
}
