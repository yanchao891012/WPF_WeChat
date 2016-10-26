using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeChat.WPF.Modules.Login.ViewModel;
using WeChat.WPF.Modules.Main.ViewModel;

namespace WeChat.WPF.Modules.ComManage
{
    class WeChatViewModelLocator
    {
        private static readonly object SyObject = new object();

        private static WeChatViewModelLocator _instance;
        /// <summary>
        /// ViewModel实例创建
        /// </summary>
        public static WeChatViewModelLocator Instance
        {
            get
            {
                lock (SyObject)
                {
                    if (_instance == null)
                    {
                        _instance = new WeChatViewModelLocator();
                    }
                }

                return _instance;
            }

            set
            {
                _instance = value;
            }
        }

        private LoginVM _loginViewModel;
        /// <summary>
        /// 登录
        /// </summary>
        public LoginVM LoginViewModel
        {
            get
            {
                return _loginViewModel ?? (_loginViewModel = new LoginVM());
            }
        }

        private MainVM _mainViewModel;
        /// <summary>
        /// 主页面
        /// </summary>
        public MainVM MainViewModel
        {
            get
            {
                return _mainViewModel ?? (_mainViewModel = new MainVM());
            }
        }
    }
}
