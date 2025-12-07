using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPBlock
{
        public class IndormationFile
        { 
             public string URL { get; set; } // url для загрузки
             public string TargetFormat { get; set; } // формат 
             public string FileName {  get; set; } // имя файла\
             public long? FileSize { get; set; } // размер файла
             public int TimeOut { get; set; } = 300; // тайм-аут запроса

        }

        public class DowloadInformation
        { 
            public long ByteReceived { get; set; } // данных полученно
            public long TotalByte { get; set; } // всего данных
            public double Percentage => TotalByte > 0 ? (ByteReceived * 100.0 / TotalByte) : 0;//  в процентах
        }

        public class DowloadResult
        { 
            public bool Success { get; set; } // успешность
            public string LocalFilePath { get; set;} //локальный путь к файлу
            public string OriginalURL { get; set; } // оригинальный url
            public string FileName { get; set; } // имя файла
            public string ErrorMessage { get; set; } // были ли ошибки
            public string ContentType { get; set; } // тип контента
        }
    }
