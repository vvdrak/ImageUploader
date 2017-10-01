using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ImageUploader.Models
{
    internal class Summator
    {
        #region MultithreadSingleton
        private static Summator instance;
        private static object syncRoot = new object();

        public static Summator GetInstance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new Summator();
                }
            }
            return instance;
        }

        private Summator()
        {
            _list = new List<Uploader>();
            _thread = new Thread(Run) { IsBackground = true };
            _thread.Start();
        }
        #endregion

        public delegate void ProgressChangedEventHandler(long current);
        public event ProgressChangedEventHandler ProgressChanged;

        private List<Uploader> _list;
        private Thread _thread;

        internal void CountFrom(Uploader uploader)
        {
            if (!_list.Contains(uploader))
                _list.Add(uploader);
        }

        internal void Forget(Uploader uploader)
        {
            if (_list.Contains(uploader))
                _list.Remove(uploader);
        }

        private void Run()
        {
            long oldPercent = 0;
            while (true)
            {
                Thread.Sleep(100);
                if (_list.All(x => x.BytesReceived != 0 && x.BytesReceived == x.TotalBytesToReceive))
                {
                    _list.Clear();
                    oldPercent = 0;
                }

                long max = 0, current = 0;
                for (int i = 0; i < _list.Count; i++)
                {
                    current += _list[i].BytesReceived;
                    max += _list[i].TotalBytesToReceive;
                }
                long percent = max != 0 ? current * 100 / max : 0;
                if (percent >= oldPercent)
                {
                    ProgressChanged(percent);
                    oldPercent = percent;
                }
            }
        }
    }
}
