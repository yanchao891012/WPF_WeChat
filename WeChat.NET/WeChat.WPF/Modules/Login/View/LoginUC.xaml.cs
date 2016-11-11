using GalaSoft.MvvmLight.Messaging;
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
using System.Windows.Shapes;
using WeChat.WPF.Modules.Main.View;

namespace WeChat.WPF.Modules.Login.View
{
    /// <summary>
    /// LoginUC.xaml 的交互逻辑
    /// </summary>
    public partial class LoginUC : Window
    {
        public LoginUC()
        {
            InitializeComponent();
            grid_content.Children.Add(new QRCodeUC());

            //收到显示登录信息广播通知
            Messenger.Default.Register<object>(this, "ShowLoginInfoUC", new Action<object>(p =>
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    AddUC(new LoginInfoUC());
                    img.Visibility = Visibility.Collapsed;
                }));
            }
            ));
            //卸载登录信息广播
            this.Unloaded += (s, e) => Messenger.Default.Unregister<object>(this, "ShowLoginInfoUC");

            //收到显示二维码信息广播通知
            Messenger.Default.Register<object>(this, "ShowQRCodeUC", new Action<object>(p =>
             {
                 this.Dispatcher.Invoke((Action)(() =>
                 {
                     AddUC(new QRCodeUC());
                     img.Visibility = Visibility.Visible;
                 }));
             }));
            //卸载二维码信息广播
            this.Unloaded += (s, e) => Messenger.Default.Unregister<object>(this, "ShowQRCodeUC");
            //收到隐藏的广播
            Messenger.Default.Register<object>(this, "HideLoginUC", new Action<object>(p =>
             {
                 this.Dispatcher.Invoke((Action)(() =>
                 {
                     MainUC mainUC = new MainUC();
                     mainUC.Show();
                     this.Hide();
                 }));
             }));
            //卸载隐藏的广播
            this.Unloaded += (s, e) => Messenger.Default.Unregister<object>(this, "HideLoginUC");
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //左键拖动
            this.DragMove();
        }
        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //退出应用程序
            Environment.Exit(0);
        }
        /// <summary>
        /// 添加新的UC
        /// </summary>
        /// <param name="uc"></param>
        private void AddUC(UserControl uc)
        {
            grid_content.Children.Clear();
            grid_content.Children.Add(uc);
        }
    }
}
