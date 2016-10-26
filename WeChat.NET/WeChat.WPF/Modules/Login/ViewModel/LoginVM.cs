using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WeChat.HTTP;
using WeChat.WPF.Modules.Main.View;

namespace WeChat.WPF.Modules.Login.ViewModel
{
    public class LoginVM : ViewModelBase
    {
        LoginService ls = new LoginService();
        System.Threading.Thread thread;
        /// <summary>
        /// 构造函数
        /// </summary>
        public LoginVM()
        {
            
        }

        #region 方法事件
        /// <summary>
        /// 获取二维码
        /// </summary>
        private void GetQRCode()
        {
            QRCodeImageSource = ls.GetQRCode();
            //启用一个新线程来循环登录
            thread = new System.Threading.Thread(new System.Threading.ThreadStart(LoopLoginCheck));
            thread.Start();
        }
        /// <summary>
        /// 循环检测是否登录了
        /// </summary>
        private void LoopLoginCheck()
        {
            object login_result = null;
            //循环判断手机扫描二维码结果
            while (true)
            {
                login_result = ls.LoginCheck();
                //已扫描 未登录
                if (login_result is ImageSource)
                {
                    HeadImageSource = login_result as ImageSource;
                    //广播，通知到LoginUC页面，切换
                    Messenger.Default.Send<object>(null, "ShowLoginInfoUC");
                }
                //已完成登录
                if (login_result is string)
                {
                    //访问登录跳转URL
                    ls.GetSidUid(login_result as string);
                                     
                    //广播，隐藏登录页面,打开主页面
                    Messenger.Default.Send<object>(null, "HideLoginUC");

                    thread.Abort();
                    break;
                }
                ////超时
                if (login_result is int)
                {
                    //QRCodeImageSource = ls.GetQRCode();
                    //返回二维码页面
                    Messenger.Default.Send<object>(null, "ShowQRCodeUC");
                }
            }
        }

        private RelayCommand _loadCommand;
        /// <summary>
        /// 载入事件
        /// </summary>
        public RelayCommand LoadCommand
        {
            get
            {
                return _loadCommand ?? (_loadCommand = new RelayCommand(() =>
                    {
                        GetQRCode();
                    }));
            }
        }

        private RelayCommand _returnQRCodeCommand;
        /// <summary>
        /// 点击返回二维码
        /// </summary>
        public RelayCommand ReturnQRCodeCommand
        {
            get
            {
                return _returnQRCodeCommand ?? (_returnQRCodeCommand = new RelayCommand(() =>
                    {
                        Messenger.Default.Send<object>(null, "ShowQRCodeUC");
                    }));
            }
        }
        #endregion

        #region 字段属性
        private ImageSource _qRCodeImageSource;
        /// <summary>
        /// 二维码图片
        /// </summary>
        public ImageSource QRCodeImageSource
        {
            get
            {
                return _qRCodeImageSource;
            }

            set
            {
                _qRCodeImageSource = value;
                RaisePropertyChanged("QRCodeImageSource");
            }
        }

        private ImageSource _headImageSource;
        /// <summary>
        /// 头像图片
        /// </summary>
        public ImageSource HeadImageSource
        {
            get
            {
                return _headImageSource;
            }

            set
            {
                _headImageSource = value;
                RaisePropertyChanged("HeadImageSource");
            }
        }
        #endregion
    }
}
