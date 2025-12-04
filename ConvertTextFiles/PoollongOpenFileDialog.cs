using Microsoft.Win32;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cONVERTPDFTEXT
{
    public class PoollongOpenFileDialog
    {
        private readonly ConcurrentQueue<OpenFileDialog> _pool = new ConcurrentQueue<OpenFileDialog>();
        private static int _maxPoolSize = 7;

        public OpenFileDialog PollOpen()
        {
            if (_pool.TryDequeue(out OpenFileDialog dialog))
            { 
                return dialog;
            }
            return new OpenFileDialog();
        }

        public void reset(OpenFileDialog save)
        {
            save.FileName = string.Empty;
            save.Filter = string.Empty;
            save.FilterIndex = 0;
            save.Title = string.Empty;
        }

        public void PoolClosed(OpenFileDialog openFileDialog)
        {
            if (openFileDialog == null) return;

            reset(openFileDialog);

            if (_pool.Count > _maxPoolSize)
            { 
                _pool.Enqueue(openFileDialog);
            }
        }
    }
}
