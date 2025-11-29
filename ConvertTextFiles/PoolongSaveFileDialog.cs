using Microsoft.Win32;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cONVERTPDFTEXT
{
    public class PoolongSaveFileDialog
    {
        private readonly ConcurrentQueue<SaveFileDialog> _pool = new ConcurrentQueue<SaveFileDialog>();
        private static int _maxPoolSize = 7;

        public SaveFileDialog GetSavefiledialog()
        {
                if (_pool.TryDequeue(out SaveFileDialog dialog))
                {
                    return dialog;
                }           
            return new SaveFileDialog();
        }

        public void RefreshFilegialog(SaveFileDialog save)
        {
            if (save == null) return;

            reset(save);
                if (_pool.Count < _maxPoolSize)
                {
                    _pool.Enqueue(save);
                }
        }

        public void reset(SaveFileDialog save)
        {
            save.FileName = string.Empty;
            save.Filter = string.Empty;
            save.FilterIndex = 0;
            save.Title = string.Empty; 
        }
    }
}
