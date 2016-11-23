using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using WeChat.Emoji;

namespace WeChat.WPF.Modules.Main.View
{
    /// <summary>
    /// MainUC.xaml 的交互逻辑
    /// </summary>
    public partial class MainUC : Window
    {
        //Rect rcnormal;
        public MainUC()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 鼠标左键拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
            ShowInTaskbar = false;
        }
        /// <summary>
        /// 最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_max_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
            ShowOrhide();
        }
        /// <summary>
        /// 正常大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_normal_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
            ShowOrhide();
        }
        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_min_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 双击最大化或者是还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                ShowOrhide();
            }
        }
        /// <summary>
        /// 显示或者隐藏按钮
        /// </summary>
        private void ShowOrhide()
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    btn_normal.Visibility = Visibility.Collapsed;
                    btn_max.Visibility = Visibility.Visible;
                    break;
                case WindowState.Minimized:
                    break;
                case WindowState.Maximized:
                    btn_max.Visibility = Visibility.Collapsed;
                    btn_normal.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        #region 系统托盘
        //系统启动的时候给个初始化为Normal
        WindowState lastWindowState = WindowState.Normal;
        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStateChanged(EventArgs e)
        {
            this.lastWindowState = WindowState == WindowState.Minimized ? lastWindowState : WindowState;
        }

        private void Open()
        {
            WindowState = lastWindowState;
            ShowInTaskbar = true;
            Show();
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void NotificationAreaIcon_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Open();
            }
        }

        #endregion

        /// <summary>
        /// 弹窗关闭，并给RichTextbox添加Emoji
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EmojiTabControlUC_Close(object sender, EventArgs e)
        {
            //将Emoji放入以后，把光标移动到刚插入的Emoji的结尾部分
            var container=new InlineUIContainer(new Image { Source = EmojiTabControlUC.SelectEmoji.Value, Height = 20, Width = 20 }, rtb.CaretPosition);
            rtb.CaretPosition = container.ElementEnd;

            rtb.Focus();
            
            pop.IsOpen = false;
        }      
    }
}
