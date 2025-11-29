using Aspose.Pdf.Operators;
using Aspose.Pdf.Text;
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
        private  long _filerazmer;
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
            var filepath = await FilePath();
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

        public async Task<string> FileSave()
        {
            PoolongSaveFileDialog pool = new PoolongSaveFileDialog();
            SaveFileDialog saveFileDialog = null;
            try
            {
                saveFileDialog = pool.GetSavefiledialog();
                saveFileDialog.Title = "Сохранение";
                saveFileDialog.Filter = "PDF файлы (*.pdf)|*.pdf|Все файлы (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filepathsave = saveFileDialog.FileName;
                    string disk = Path.GetPathRoot(filepathsave);
                    DriveInfo drive = new DriveInfo(disk);
                    long freeSpace = drive.AvailableFreeSpace;
                    if (_filerazmer > freeSpace)
                    {
                        return "На диске не достаточно свобожного места";
                    }

                    string pathgetex = Path.GetExtension(filepathsave);
                    if (pathgetex.ToLower() == ".pdf")
                    {
                        await convertpdf(filepathsave);
                        MessageBox.Show($"Файл успешно сохранен по пути {filepathsave}");
                        return filepathsave;
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
