using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeChat.Controls
{
    /// <summary>
    /// FriendsListBox.xaml 的交互逻辑
    /// </summary>
    public partial class FriendsListBox : ListBox
    {
        public FriendsListBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 图片
        /// </summary>
        private ImageBrush _imgSource;
        /// <summary>
        /// 用户名
        /// </summary>
        private string _userName;
        /// <summary>
        /// 图片
        /// </summary>
        public ImageBrush ImgSource
        {
            get
            {
                return _imgSource;
            }

            set
            {
                _imgSource = value;
            }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get
            {
                return _userName;
            }

            set
            {
                _userName = value;
            }
        }
    }
}
