using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace COMMON
{
    public partial class BaseForm : Form
    {

        #region 全局变量

        private AutoValidate preCloseAutoValidate = AutoValidate.EnablePreventFocusChange;
        public StartPauseStop sps = new StartPauseStop();
        private delegate void SendPaste();
        protected INIClass ini;
        //坐标
        private Coordinate _p;
        public Coordinate P
        {
            get { return _p; }
            set { _p = value; }
        }
        private bool IsStop= false;
        #endregion

        #region 构造函数
        public BaseForm()
        {
            InitializeComponent();
        }
        #endregion

        #region 画面初期化
        private void BaseForm_Load(object sender, EventArgs e)
        {
            // 注册热键
            bool isSucs = HotKey.RegisterHotKey(Handle, 100, HotKey.KeyModifiers.None, Keys.F10);
            isSucs = HotKey.RegisterHotKey(Handle, 101, HotKey.KeyModifiers.None, Keys.F11);

            // 指定开启事件
            sps.DoSomethingEvent += new StartPauseStop.DoSomething(MainLogic);

            // 加载配置文件
            ini = new INIClass(System.IO.Directory.GetCurrentDirectory() + "\\config.ini");

            // 进行初期化设置
            Init();
        }
        #endregion

        #region API函数

        #region 设置光标位置
        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        protected extern static int SetCursorPos(int x, int y);
        #endregion

        #region 模拟鼠标单击
        [DllImport("user32")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        private static readonly int MOUSEEVENTF_LEFTDOWN = 0x2; //模拟鼠标左键按下
        private static readonly int MOUSEEVENTF_LEFTUP = 0x4; //模拟鼠标左键释放
        #endregion

        #region sleep
        [DllImport("kernel32.DLL", EntryPoint = "Sleep")]
        internal extern static void Sleep(int dwMilliseconds);
        #endregion

        #region 窗口信息
        [DllImport("user32.dll")]//获得窗口句柄
        public static extern int WindowFromPoint(int xPoint, int yPoint);

        [DllImport("user32.dll")]//获得窗口类
        public static extern int GetClassName(int hwnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32", EntryPoint = "FindWindowA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr FindWindow([MarshalAs(UnmanagedType.VBByRefStr)] ref string IpClassName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string IpWindowName);

        [DllImport("user32.dll")]
        public static extern int PostMessage(int hwnd, int msg, IntPtr wparam, IntPtr lparam);

        // 获取窗口名称
        [DllImport("user32.dll", EntryPoint = "GetWindowText")]
        public static extern int GetWindowText(int hwnd, StringBuilder lpString,int cch);

        #endregion


        #endregion

        #region 事件

        protected void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {

            if (!this.Visible)
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                //inRoom = false;
                sps.Stop();
                IsStop = true;
                MessageBox.Show("程序已经停止，如要重新开始，请返回游戏初始状态！", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        protected void BaseForm_Resize(object sender, EventArgs e)
        {
            //if (this.WindowState == FormWindowState.Minimized)
            //{
            //    //this.Visible = false;
            //}
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!FormCheck())
            {
                return;
            }

            Start();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        #endregion

        #region 继承的方法
        protected override void WndProc(ref Message m)
        {
            //画面を閉じる直前（OnClosingメソッドの呼び出しより前）
            if (m.Msg == 0x0010 ||    // Win32Msg.WM_CLOSE
                m.Msg == 0x0011 ||      // Win32Msg.WM_QUERYENDSESSION
                m.Msg == 0x0016)    // Win32Msg.WM_ENDSESSION
            {
                //暗黙的な検証（AutoValidation）の無効化
                DisableAutoValidateForClosing(true);
            }

            const int WM_HOTKEY = 0x0312;
            //按快捷键    
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:    //按下的是F10
                            btnStart_Click(null, null);
                            break;
                        case 101:    //按下的是F12  
                            sps.Stop();
                            MessageBox.Show("程序已经停止，如要重新开始，请返回游戏初始状态！", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Visible = true;
                            this.WindowState = FormWindowState.Normal;
                            break;
                    }
                    break;
                default:
                    string ss = string.Empty;
                    break;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// 画面を閉じる際、暗黙的な検証（AutoValidation）を無効に設定するための操作です。
        /// </summary>
        /// <param name="disable">無効にする場合には true、解除する場合には false を指定</param>
        private void DisableAutoValidateForClosing(bool disable)
        {
            // 【対処４】Form.Closeメソッドで画面を閉じる
            // 【対処５】キャプションバーの[×]ボタンで画面を閉じる
            // 【対処６】[Alt]+[F4]キー（ショートカット）で画面を閉じる

            // モードレス（一次ウィンドウ含む）のみを対象とする
            if (Modal)
                return;

            // 無効
            if (disable)
            {
                // 暗黙的な検証の無効化
                preCloseAutoValidate = AutoValidate;
                AutoValidate = AutoValidate.Disable;
            }
            // 復帰
            else
            {
                // 暗黙的な検証の無効化を解除
                AutoValidate = preCloseAutoValidate;
            }
        }

        /// <summary>
        /// System.Windows.Forms.Form.FormClosing イベントを発生させます
        /// </summary>
        /// <param name="e">
        /// イベント データを格納している System.Windows.Forms.FormClosingEventArgs
        /// </param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            //復帰（[閉じる]のをキャンセルした場合）
            if (e.Cancel)
            {
                // 暗黙的な検証（AutoValidation）の無効化を解除
                DisableAutoValidateForClosing(false);
            }
        }

        #endregion

        #region 虚方法

        /// <summary>
        /// 画面Check
        /// </summary>
        /// <returns></returns>
        protected virtual bool FormCheck()
        {
            return true;
        }

        protected virtual void Start()
        {
            SetValue();
            IsStop = false;
            this.Visible = false;
            sps.Start();
            if (this.Visible)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        protected virtual void SetValue()
        {
            //if (this.cmbCoo.SelectedIndex == 0)
            //{
            //    P = new coordinate(coordinate.分辨率._1024_768);
            //}
            //else
            //{
            //    P = new coordinate(coordinate.分辨率._1440_900);
            //}
        }

        /// <summary>
        /// 主逻辑
        /// </summary>
        /// <param name="myObject"></param>
        /// <param name="myEventArgs"></param>
        protected virtual void MainLogic(Object myObject, EventArgs myEventArgs)
        {

        }

        /// <summary>
        /// 粘贴
        /// </summary>
        protected virtual void SendPasteContext()
        {
            System.Windows.Forms.SendKeys.SendWait("<B><FONT SIZE=" + "4" + " COLOR=" + "Red" + ">" + "aaa" + "</FONT></B>");
        }

        /// <summary>
        /// 等待时间
        /// </summary>
        /// <param name="s"></param>
        protected void 等待时间(double s)
        {
            Thread.Sleep((int)(s * 1000));
        }

        /// <summary>
        /// 鼠标单击
        /// </summary>
        /// <param name="s"></param>
        protected void 鼠标单击()
        {

            等待时间(0.1);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            等待时间(0.1);
        }

        /// <summary>
        /// 鼠标双击
        /// </summary>
        /// <param name="s"></param>
        protected void 鼠标双击()
        {
            等待时间(0.1);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            等待时间(0.3);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            等待时间(0.1);
        }

        private void MySendPaste()
        {
            SendPaste s = new SendPaste(MySendPaste);
            IAsyncResult iftAR = s.BeginInvoke(null, null);
            s.EndInvoke(iftAR);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="s"></param>
        protected virtual void Save()
        {
            if (ini.ExistINIFile())
            {
                ini.IniWriteValue("喊话内容", "喊话内容", this.txtContext.Text);
            }
            else
            {
                MessageBox.Show("配置文件已丢失！", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="s"></param>
        protected virtual void Init()
        {
            if (ini.ExistINIFile())
            {
                string str1 = ini.IniReadValue("喊话内容", "喊话内容");
                this.txtContext.Text = str1;

            }
        }
        #endregion

        private void BaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            HotKey.UnregisterHotKey(Handle, 100);
            HotKey.UnregisterHotKey(Handle, 101);
        }

    }
}
