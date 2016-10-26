using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeChat.WPF.Modules.Main.Model
{
    /// <summary>
    /// 微信消息
    /// </summary>
    public class WeChatMsg
    {
        private string _from;
        private string _to;
        private DateTime _time;
        private bool _readed;
        private string _msg;
        private int _type;
        /// <summary>
        /// 消息发送方
        /// </summary>
        public string From
        {
            get
            {
                return _from;
            }

            set
            {
                _from = value;
            }
        }
        /// <summary>
        /// 消息接收方
        /// </summary>
        public string To
        {
            get
            {
                return _to;
            }

            set
            {
                _to = value;
            }
        }
        /// <summary>
        /// 消息发送时间
        /// </summary>
        public DateTime Time
        {
            get
            {
                return _time;
            }

            set
            {
                _time = value;
            }
        }
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool Readed
        {
            get
            {
                return _readed;
            }

            set
            {
                _readed = value;
            }
        }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Msg
        {
            get
            {
                return _msg;
            }

            set
            {
                _msg = value;
            }
        }
        /// <summary>
        /// 消息类型
        /// </summary>
        public int Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
            }
        }
    }
}
