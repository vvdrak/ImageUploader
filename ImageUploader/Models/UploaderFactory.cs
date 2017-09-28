using System;
using System.Collections.Generic;

namespace ImageUploader.Models
{
    internal sealed class UploaderFactory
    {
        #region Multithread singleton
        private static UploaderFactory _instance;
        private static object syncRoot = new object();

        public static UploaderFactory GetInstance()
        {
            if (_instance == null)
            {
                lock (syncRoot)
                {
                    if (_instance == null)
                        _instance = new UploaderFactory();
                }
            }
            return _instance;
        }

        private UploaderFactory()
        {
            _queue = new Stack<Uploader>();
        }
        #endregion

        private Stack<Uploader> _queue;

        public Uploader GetNewUploader()
        {
            if (_queue.Count > 3)
                _queue.Clear();

            _queue.Push(new Uploader());
            return _queue.Peek();
        }

        public Tuple<Uploader, Uploader, Uploader> GetWorkingImpl() =>
            new Tuple<Uploader, Uploader, Uploader>(_queue.Pop(), _queue.Pop(), _queue.Pop());
    }
}
