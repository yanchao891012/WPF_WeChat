using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace WeChat.WPF.Modules.Main.Model
{
    public class ChatMsg
    {

        private ImageSource _image;

        public ImageSource Image
        {
            get
            {
                return _image;
            }

            set
            {
                _image = value;
            }
        }

        private FlowDocument _message=new FlowDocument();
        /// <summary>
        /// 显示的内容
        /// </summary>
        public FlowDocument Message
        {
            get
            {
                return _message;
            }

            set
            {
                _message = value;
            }
        }

        private FlowDirection _flowDir;
        /// <summary>
        /// 显示方向
        /// </summary>
        public FlowDirection FlowDir
        {
            get
            {
                return _flowDir;
            }

            set
            {
                _flowDir = value;
            }
        }

        private Brush _tbColor;
        /// <summary>
        /// 显示的背景颜色
        /// </summary>
        public Brush TbColor
        {
            get
            {
                return _tbColor;
            }

            set
            {
                _tbColor = value;
            }
        }
    }
}
