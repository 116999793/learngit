using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using COMMON;
using System.Diagnostics;
using System.Collections;

namespace _17PK
{
    public partial class Frm17PK : COMMON.BaseForm
    {

        #region 全局变量

        // CheckBox列表
        private List<System.Windows.Forms.CheckBox> _list = new List<CheckBox>();

        // 桌子数量
        private const int TABLE_COUNT = 60;

        // 当前桌子号码
        private int tableNum = 0;

        private const int Y_差值 = 190;


        #endregion

        #region 构造函数
        public Frm17PK()
        {
            InitializeComponent();
        }
        #endregion

        #region 程序开始函数
        /// <summary>
        /// 主逻辑
        /// </summary>
        /// <param name="myObject"></param>
        /// <param name="myEventArgs"></param>
        protected override void MainLogic(Object myObject, EventArgs myEventArgs)
        {
            try
            {
                for (int i = 0; i <= 11; i++) //12个房间
                {
                    if (_list[i].Checked != true)
                    {
                        continue;
                    }
                    进入房间(i * 20); //20是房间位置偏移量
                    tableNum = 0;
                    for (int k = 0; k < 21; k++) //23排桌子
                    {

                        for (int j = 0; j <= 2; j++) //每排有3张桌子
                        {
                            tableNum += 1;
                            if (tableNum > TABLE_COUNT)
                                continue;
                            //北风
                            if (!进入游戏桌(j * 176))
                                continue;

                            // 判断游戏是否开始
                            Point reject = 检查画面有无消息("不同意", "不同意", true);
                            if (!reject.IsEmpty)
                            {
                                SetCursorPos(reject.X, reject.Y);
                                鼠标单击();
                                continue;
                            }

                            喊话();

                            退出游戏桌();
                            
                        }
                        if (tableNum > TABLE_COUNT)
                            continue;

                        //下一排
                        等待时间(5);
                        SetCursorPos(P.X_滚动条, P.Y_滚动条);
                        等待时间(0.5);
                        for (int m = 1; m <= 19; m++)
                        {
                            等待时间(0.3);
                            鼠标单击();
                        }
                        等待时间(3);
                        if (k == 20)
                            进行最后三排桌子的喊话();
                    }
                    退出房间();

                }
            }
            catch
            {
            }
        }
        #endregion

        #region 内部方法

        #region 值设定
        protected override void SetValue()
        {
            P = new Coordinate(Coordinate.分辨率._1024_768);
        }
        #endregion 

        #region 画面检查
        protected override bool FormCheck()
        {
            _list.Clear();
            _list.Add(this.房间1);
            _list.Add(this.房间2);
            _list.Add(this.房间3);
            _list.Add(this.房间4);
            _list.Add(this.房间5);
            _list.Add(this.房间6);
            _list.Add(this.房间7);
            _list.Add(this.房间8);
            _list.Add(this.房间9);
            _list.Add(this.房间10);
            _list.Add(this.房间11);
            _list.Add(this.房间12);
            bool hasRoom = false;
            foreach (CheckBox c in _list)
            {
                if (c.Checked == true)
                {
                    hasRoom = true;
                    break;
                }
            }
            if (!hasRoom)
            {
                //至少选中一个房间
                MessageBox.Show("至少需要选中一个房间!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                return false;
            }

            return 检查程序是否运行();
        }
        #endregion

        #region 检查程序是否运行
        protected bool 检查程序是否运行()
        {

            Process[] processes;
            processes = System.Diagnostics.Process.GetProcesses();
            for (int i = 0; i < processes.Length - 1; i++)
            {
                if (processes[i].ProcessName == "GamePlaza")
                    return true;
            }
            MessageBox.Show("请先运行游戏!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            return false;
        }

        #endregion

        #region 进入房间
        private void 进入房间(int y)
        {
            等待时间(1);
            SetCursorPos(P.X_房间, P.Y_房间 + y);
            等待时间(0.5);
            鼠标单击();
            等待时间(10);
        }
        #endregion

        #region 进入游戏桌
        /// <summary>
        /// 进入游戏桌
        /// </summary>
        /// <param name="x"></param>
        private bool 进入游戏桌(int x, params int[] y)

        {
            for(int i=0;i<4;i++)
            {
                等待时间(1);
                if (y.Length > 0)
                {
                    SetCursorPos(P.X_第一桌[i] + x, y[0] + P.Y_第一桌[i]);
                }
                else
                {
                    SetCursorPos(P.X_第一桌[i] + x, P.Y_第一桌[i]);
                }
                等待时间(0.5);
                鼠标单击();

                等待时间(2);

                Point cancle = 检查画面有无消息("银子不足", "取消", true);

                if (!cancle.IsEmpty)
                {
                    SetCursorPos(cancle.X, cancle.Y);
                    鼠标单击();
                    return false;
                }

                Point taishao = 检查画面有无消息("银两太少", "取消", true);

                if (!taishao.IsEmpty)
                {
                    SetCursorPos(taishao.X, taishao.Y);
                    鼠标单击();
                    return false;
                }


                Point setbufu = 检查画面有无消息("设置不符", "取消", true);

                if (!setbufu.IsEmpty)
                {
                    SetCursorPos(setbufu.X, setbufu.Y);
                    鼠标单击();
                    return false;
                }
                Point zzmm = 检查画面有无消息("桌子密码", "取消", true);

                if (!zzmm.IsEmpty)
                {
                    SetCursorPos(zzmm.X, zzmm.Y);
                    鼠标单击();
                    return false;
                }
                Point ok = 检查画面有无消息("下次动作要快点", "确定", true);
                if (!ok.IsEmpty)
                {
                    SetCursorPos(ok.X, ok.Y);
                    鼠标单击();
                    return false;
                }

                if (Process.GetProcessesByName("GuanDanHA").Length > 0)
                    return true;
                
            }
            return false;

        }
        #endregion

        #region 退出房间
        /// <summary>
        /// 退出房间
        /// </summary>
        private void 退出房间()
        {
            等待时间(2);
            SetCursorPos(P.X_退出房间, P.Y_退出房间);
            鼠标单击();
            等待时间(1);

            Point isOK = 检查画面有无消息("确定退出吗", "确定(", true);
            if (!isOK.IsEmpty)
            {
                SetCursorPos(isOK.X, isOK.Y);
                鼠标单击();
            }
        }
        #endregion

        #region 退出游戏桌&广告
        /// <summary>
        /// 退出游戏桌
        /// </summary>
        private void 退出游戏桌()
        {
            bool isTrueExist = false;
            while (!isTrueExist)
            {
                等待时间(5);

                SetCursorPos(P.X_退出桌, P.Y_退出桌);
                鼠标单击();
                等待时间(0.5);
                isTrueExist = Process.GetProcessesByName("GuanDanHA").Length == 0;
            }

        }
        #endregion

        #region 喊话
        /// <summary>
        /// 喊话
        /// </summary>
        private void 喊话()
        {
            等待时间(5);

            Point cancle = 检查画面有无消息("银子不足", "取消", true);

            if (!cancle.IsEmpty)
            {
                SetCursorPos(cancle.X, cancle.Y);
                鼠标单击();
                return;
            }

            Point taishao = 检查画面有无消息("银两太少", "取消", true);

            if (!taishao.IsEmpty)
            {
                SetCursorPos(taishao.X, taishao.Y);
                鼠标单击();
                return;
            }


            Point setbufu = 检查画面有无消息("设置不符", "取消", true);

            if (!setbufu.IsEmpty)
            {
                SetCursorPos(setbufu.X, setbufu.Y);
                鼠标单击();
                return;
            }
            Point set = 检查画面有无消息("游戏设置", "设置", false);
            if (!set.IsEmpty)
            {
                SetCursorPos(set.X, set.Y);
                鼠标单击();
            }

            SetCursorPos(P.X_发言栏, P.Y_发言栏);
            鼠标单击();
            SendKeys.SendWait(txtContext.Text);
            等待时间(1);
            SendKeys.SendWait("{Enter}");
            等待时间(2);
        }
        #endregion

        #region 保存按钮
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ini.ExistINIFile())
            {
                if(分辨率1.Checked)
                    ini.IniWriteValue("分辨率", "分辨率", 分辨率1.Name);
                else if (分辨率2.Checked)
                    ini.IniWriteValue("分辨率", "分辨率", 分辨率2.Name);

                if (rdo掼蛋.Checked)
                    ini.IniWriteValue("游戏", "游戏", rdo掼蛋.Name);

                string roomNum = string.Empty;

                foreach (Control ctl in groupBox4.Controls)
                {
                    if (((CheckBox)ctl).Checked)
                    {
                        if (!string.IsNullOrEmpty(roomNum))
                            roomNum = roomNum + "," + ctl.Name.Substring(2, ctl.Name.Length - 2);
                        else
                            roomNum= ctl.Name.Substring(2, ctl.Name.Length - 2);
                    }
                }
                ini.IniWriteValue("房间", "房间", roomNum);

            }
            else
            {
                MessageBox.Show("配置文件已丢失！", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 画面初期化
        private void Frm17PK_Load(object sender, EventArgs e)
        {
            if (ini.ExistINIFile())
            {
                string strfbl = ini.IniReadValue("分辨率", "分辨率");
                string strGame = ini.IniReadValue("游戏", "游戏");
                string strRoom = ini.IniReadValue("房间", "房间");

                if (!string.IsNullOrEmpty(strfbl))
                    // 设置分辨率
                    ((RadioButton)groupBox3.Controls[strfbl]).Checked = true;

                if (!string.IsNullOrEmpty(strGame))
                    // 游戏选择
                    ((RadioButton)groupBox1.Controls[strGame]).Checked = true;

                if (!string.IsNullOrEmpty(strRoom))
                {
                    foreach (string room in strRoom.Split(','))
                    {
                        ((CheckBox)groupBox4.Controls["房间" + room]).Checked = true;
                    }
                }
            }
        }
        #endregion

        #region 检查画面有无消息
        public Point 检查画面有无消息(string queString,string ok_Or_cancle,bool isLike)
        {
            // 获取所有窗体标题及坐标
            Hashtable hsTitle = GetAllTitlePoint();
            bool haveMsg = false;

            // 查看有无弹出错误消息
            foreach (string keys in hsTitle.Keys)
            {
                if (keys.IndexOf(queString) >= 0)
                    haveMsg = true;
            }

            // 没有问题时，返回空坐标
            if (!haveMsg)
                return Point.Empty;

            // 查看有无弹出错误消息
            foreach (string keys in hsTitle.Keys)
            {
                if (isLike && keys.IndexOf(ok_Or_cancle) >= 0)
                    return (Point)hsTitle[keys];
                else if (!isLike)
                    return (Point)hsTitle[ok_Or_cancle];
            }

            return Point.Empty;

        }
        #endregion

        #region 获取所有窗体标题及坐标
        private Hashtable GetAllTitlePoint()
        {
            Hashtable returnList = new Hashtable();
            // 获取所有窗体的标题名称
            for (int x = 1; x < Screen.PrimaryScreen.Bounds.Width; x = x + 10)
            {
                for (int y = 1; y < Screen.PrimaryScreen.Bounds.Height; y = y + 10)
                {

                    // 获取该坐标程序句柄
                    int handle = WindowFromPoint(x, y);
                    StringBuilder title = new StringBuilder(256);
                    //得到窗口的标题
                    GetWindowText(handle, title, 100);

                    if (!returnList.ContainsKey(title.ToString()))
                        returnList.Add(title.ToString(), new Point(x, y));
                }
            }
            return returnList;
        }

        #endregion

        #region 最后一桌方法

        private void 进行最后三排桌子的喊话()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j <= 2; j++) //每排有3张桌子
                {
                    tableNum += 1;
                    if (tableNum > TABLE_COUNT)
                        continue;
                    //北风
                    if (!进入游戏桌(j * 176, new int[] { 30 + (Y_差值 * i) }))
                        continue;

                    // 判断游戏是否开始
                    Point reject = 检查画面有无消息("不同意", "不同意", true);
                    if (!reject.IsEmpty)
                    {
                        SetCursorPos(reject.X, reject.Y);
                        鼠标单击();
                        continue;
                    }

                    喊话();

                    退出游戏桌();
                }
                等待时间(3);
            }
        }
        #endregion

        #endregion

    }
}
