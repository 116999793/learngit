using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace COMMON
{
    public sealed class StartPauseStop
    {
        bool _threadSwitch = false;
        bool _pauseSwitch = false;
        AutoResetEvent _resetEvent = new AutoResetEvent(false);
        public Thread _worker = null;
        object _locker = new object();

        public bool IsStop { get; set; }
        public delegate void DoSomething(object sender, EventArgs e);

        public event DoSomething DoSomethingEvent;

        private void OnDoSomethingEvent()
        {
            try
            {
                if (DoSomethingEvent != null)
                {
                    DoSomethingEvent(this, new EventArgs());
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public void Start()
        {
            lock (_locker)
            {
                if (_worker == null)
                {
                    _threadSwitch = true;
                    _worker = new Thread(Run);
                    _worker.IsBackground = true;
                    _worker.Start();
                }
            }

        }

        void Run()
        {
            while (_threadSwitch)
            {
                try
                {
                    OnDoSomethingEvent();
                    if (_pauseSwitch)
                    {
                        _resetEvent.WaitOne();
                    }
                    IsStop = false;
                }
                catch //(System.Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
                Thread.Sleep(0);
            }
        }

        public void Continue()
        {
            lock (_locker)
            {
                if (!_pauseSwitch)
                {
                    return;
                }
                _pauseSwitch = false;
                _resetEvent.Set();
            }
        }

        public void Pause()
        {
            lock (_locker)
            {
                if (_pauseSwitch)
                {
                    return;
                }
                _pauseSwitch = true;
            }
        }

        public void Stop()
        {
            IsStop = true;
            lock (_locker)
            {
                if (_worker == null)
                {
                    return;
                }
                _threadSwitch = false;
                _pauseSwitch = false;
                _resetEvent.Set();
                if (_worker.IsAlive)
                {
                    try
                    {
                        _worker.Abort();
                    }
                    catch
                    {
                    }
                }
                _worker = null;
            }
        }
    }
}
