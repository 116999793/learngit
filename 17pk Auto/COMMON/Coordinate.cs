using System;
using System.Collections.Generic;
using System.Text;

namespace COMMON
{
    public class Coordinate
    {
        public enum 分辨率
        {
            _1024_768,
            _1440_900
        }

        /// <summary>
        /// 房间地址X
        /// </summary>
        public int X_房间 { get; set; }
        /// <summary>
        /// 房间地址Y
        /// </summary>
        public int Y_房间 { get; set; }

        /// <summary>
        /// 第一桌地址X
        /// </summary>
        public int[] X_第一桌 { get; set; }
        /// <summary>
        /// 第一桌地址Y
        /// </summary>
        public int[] Y_第一桌 { get; set; }

        /// <summary>
        /// 滚动条地址X
        /// </summary>
        public int X_滚动条 { get; set; }
        /// <summary>
        /// 滚动条地址Y
        /// </summary>
        public int Y_滚动条 { get; set; }

        /// <summary>
        /// 退出房间地址X
        /// </summary>
        public int X_退出房间 { get; set; }
        /// <summary>
        /// 退出房间地址Y
        /// </summary>
        public int Y_退出房间 { get; set; }

        /// <summary>
        /// 退出桌地址X
        /// </summary>
        public int X_退出桌 { get; set; }
        /// <summary>
        /// 退出桌地址Y
        /// </summary>
        public int Y_退出桌 { get; set; }

        /// <summary>
        /// 发言栏地址X
        /// </summary>
        public int X_发言栏 { get; set; }
        /// <summary>
        /// 发言栏地址Y
        /// </summary>
        public int Y_发言栏 { get; set; }

        /// <summary>
        /// 回车地址X
        /// </summary>
        public int X_回车 { get; set; }
        /// <summary>
        /// 回车地址Y
        /// </summary>
        public int Y_回车 { get; set; }
        
        public Coordinate(分辨率 p)
        {
            switch (p)
            {
                case 分辨率._1024_768:
                    X_房间 = 173;
                    Y_房间 = 256;
                    X_第一桌 = new int[] { 136, 174, 236, 206 };
                    Y_第一桌 = new int[] { 212, 174, 193, 216 };
                    X_滚动条 = 720;
                    Y_滚动条 = 717;
                    X_退出房间 = 700;
                    Y_退出房间 = 114;
                    X_退出桌 = 990;
                    Y_退出桌 = 70;
                    X_发言栏 = 830;
                    Y_发言栏 = 714;
                    X_回车 = 1007;
                    Y_回车 = 713;
                    break;
                case 分辨率._1440_900:
                    break;
            }
        }

    }


}
