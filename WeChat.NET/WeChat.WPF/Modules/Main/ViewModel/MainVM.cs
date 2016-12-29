using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using WeChat.HTTP;
using WeChat.WPF.Modules.Main.Model;
using System.Collections.ObjectModel;
using WeChat.Tools.Helpers;
using System.Windows.Threading;
using WeChat.Emoji;
using System.Windows.Documents;
using System.Drawing;
using System.Text.RegularExpressions;

namespace WeChat.WPF.Modules.Main.ViewModel
{
    public class MainVM : ViewModelBase
    {
        WeChatService wcs = new WeChatService();
        System.Timers.Timer timer = new System.Timers.Timer();

        public MainVM()
        {
            timer.Interval = 2000;
            timer.Elapsed += Timer_Elapsed;
            Init();
        }

        #region 字段属性
        /// <summary>
        /// 当前登录微信用户
        /// </summary>
        private WeChatUser _me;
        /// <summary>
        /// 当前登录微信用户
        /// </summary>
        public WeChatUser Me
        {
            get
            {
                return _me;
            }

            set
            {
                _me = value;
                RaisePropertyChanged("Me");
            }
        }

        private WeChatUser _friendUser;
        /// <summary>
        ///  聊天好友
        /// </summary>
        public WeChatUser FriendUser
        {
            get
            {
                return _friendUser;
            }

            set
            {
                if (value != _friendUser)
                {
                    if (_friendUser != null)
                    {
                        _friendUser.MsgRecved -= new WeChatUser.MsgRecvedEventHandler(_friendUser_MsgRecved);
                        _friendUser.MsgSent -= new WeChatUser.MsgSentEventHandler(_friendUser_MsgSent);
                    }
                    _friendUser = value;
                    if (_friendUser != null)
                    {
                        _friendUser.MsgRecved += new WeChatUser.MsgRecvedEventHandler(_friendUser_MsgRecved);
                        _friendUser.MsgSent += new WeChatUser.MsgSentEventHandler(_friendUser_MsgSent);
                        IEnumerable<KeyValuePair<Guid, WeChatMsg>> dic = _friendUser.RecvedMsg.Concat(_friendUser.SentMsg);
                        //dic = dic.OrderBy(p => p.Key);
                        foreach (KeyValuePair<Guid, WeChatMsg> p in dic)
                        {
                            if (p.Value.From == _friendUser.UserName)
                            {
                                ShowReceiveMsg(p.Value);
                            }
                            else
                            {
                                ShowSendMsg(p.Value);
                            }
                            p.Value.Readed = true;
                            _friendUser.UnReadCount = 0;//读了以后，就清除                         
                        }
                    }
                }
                RaisePropertyChanged("FriendUser");
            }
        }

        private WeChatUser _friendInfo;
        /// <summary>
        /// 好友信息
        /// </summary>
        public WeChatUser FriendInfo
        {
            get
            {
                return _friendInfo;
            }

            set
            {
                _friendInfo = value;
                RaisePropertyChanged("FriendInfo");
            }
        }

        /// <summary>
        /// 所有好友列表
        /// </summary>
        private List<object> _contact_all = new List<object>();
        /// <summary>
        /// 通讯录
        /// </summary>
        public List<object> Contact_all
        {
            get
            {
                return _contact_all;
            }

            set
            {
                _contact_all = value;
                RaisePropertyChanged("Contact_all");
            }
        }
        /// <summary>
        /// 部分好友列表
        /// </summary>
        private ObservableCollection<object> _contact_latest = new ObservableCollection<object>();
        /// <summary>
        /// 最近联系人
        /// </summary>
        public ObservableCollection<object> Contact_latest
        {
            get
            {
                return _contact_latest;
            }

            set
            {
                _contact_latest = value;
                RaisePropertyChanged("Contact_latest");
            }
        }

        private object _select_Contact_latest = new object();
        /// <summary>
        /// 聊天列表选中
        /// </summary>
        public object Select_Contact_latest
        {
            get
            {
                return _select_Contact_latest;
            }

            set
            {
                _select_Contact_latest = value;
                RaisePropertyChanged("Select_Contact_latest");
            }
        }

        private object _select_Contact_all = new object();
        /// <summary>
        /// 通讯录选中
        /// </summary>
        public object Select_Contact_all
        {
            get
            {
                return _select_Contact_all;
            }

            set
            {
                _select_Contact_all = value;
                RaisePropertyChanged("Select_Contact_all");
            }
        }

        private string _userName = string.Empty;
        /// <summary>
        /// 用于在顶部显示用户名
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
                RaisePropertyChanged("UserName");
            }
        }

        private ObservableCollection<ChatMsg> chatList = new ObservableCollection<ChatMsg>();
        /// <summary>
        /// 聊天列表
        /// </summary>
        public ObservableCollection<ChatMsg> ChatList
        {
            get
            {
                return chatList;
            }

            set
            {
                chatList = value;
                RaisePropertyChanged("ChatList");
            }
        }
        /// <summary>
        /// 发送消息内容
        /// </summary>
        private string _sendMessage;
        public string SendMessage
        {
            get
            {
                return _sendMessage;
            }

            set
            {
                _sendMessage = value;
                RaisePropertyChanged("SendMessage");
            }
        }

        private FlowDocument _showSendMessage = new FlowDocument();
        /// <summary>
        /// 发送框显示的发送内容
        /// </summary>
        public FlowDocument ShowSendMessage
        {
            get
            {
                return _showSendMessage;
            }

            set
            {
                _showSendMessage = value;
                RaisePropertyChanged("ShowSendMessage");
            }
        }

        private Visibility tootip_Visibility = Visibility.Collapsed;
        /// <summary>
        /// 是否显示提示
        /// </summary>
        public Visibility Tootip_Visibility
        {
            get
            {
                return tootip_Visibility;
            }

            set
            {
                tootip_Visibility = value;
                RaisePropertyChanged("Tootip_Visibility");
            }
        }

        private bool _isChecked = true;
        /// <summary>
        /// 是否被选中
        /// </summary>
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }

            set
            {
                _isChecked = value;
                RaisePropertyChanged("IsChecked");
            }
        }

        private Visibility emoji_Visibility = Visibility.Collapsed;
        /// <summary>
        /// Emoji显隐
        /// </summary>
        public Visibility Emoji_Visibility
        {
            get
            {
                return emoji_Visibility;
            }

            set
            {
                emoji_Visibility = value;
                RaisePropertyChanged("Emoji_Visibility");
            }
        }

        private bool emoji_Popup = false;
        /// <summary>
        /// Popup是否弹出
        /// </summary>
        public bool Emoji_Popup
        {
            get
            {
                return emoji_Popup;
            }

            set
            {
                emoji_Popup = value;
                RaisePropertyChanged("Emoji_Popup");
            }
        }
           
        #endregion

        #region 方法
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            //初始化
            JObject init_result = wcs.WeChatInit();

            List<object> contact_all = new List<object>();
            if (init_result != null)
            {
                _me = new WeChatUser();
                _me.UserName = init_result["User"]["UserName"].ToString();
                _me.City = "";
                _me.HeadImgUrl = init_result["User"]["HeadImgUrl"].ToString();
                _me.NickName = init_result["User"]["NickName"].ToString();
                _me.Province = "";
                _me.PyQuanPin = init_result["User"]["PYQuanPin"].ToString();
                _me.RemarkName = init_result["User"]["RemarkName"].ToString();
                _me.RemarkPYQuanPin = init_result["User"]["RemarkPYQuanPin"].ToString();
                _me.Sex = init_result["User"]["Sex"].ToString();
                _me.Signature = init_result["User"]["Signature"].ToString();
                _me.Icon = GetIcon(wcs, _me.UserName);
                //部分好友名单
                foreach (JObject contact in init_result["ContactList"])
                {
                    WeChatUser user = new WeChatUser();
                    user.UserName = contact["UserName"].ToString();
                    user.City = contact["City"].ToString();
                    user.HeadImgUrl = contact["HeadImgUrl"].ToString();
                    user.NickName = contact["NickName"].ToString();
                    user.Province = contact["Province"].ToString();
                    user.PyQuanPin = contact["PYQuanPin"].ToString();
                    user.RemarkName = contact["RemarkName"].ToString();
                    user.RemarkPYQuanPin = contact["RemarkPYQuanPin"].ToString();
                    user.Sex = contact["Sex"].ToString();
                    user.Signature = contact["Signature"].ToString();
                    user.Icon = GetIcon(wcs, user.UserName);
                    user.SnsFlag = contact["SnsFlag"].ToString();
                    user.KeyWord = contact["KeyWord"].ToString();
                    _contact_latest.Add(user);
                }
            }
            //通讯录
            JObject contact_result = wcs.GetContact();
            if (contact_all != null)
            {
                foreach (JObject contact in contact_result["MemberList"])  //完整好友名单
                {
                    WeChatUser user = new WeChatUser();
                    user.UserName = contact["UserName"].ToString();
                    user.City = contact["City"].ToString();
                    user.HeadImgUrl = contact["HeadImgUrl"].ToString();
                    user.NickName = contact["NickName"].ToString();
                    user.Province = contact["Province"].ToString();
                    user.PyQuanPin = contact["PYQuanPin"].ToString();
                    user.RemarkName = contact["RemarkName"].ToString();
                    user.RemarkPYQuanPin = contact["RemarkPYQuanPin"].ToString();
                    user.Sex = contact["Sex"].ToString();
                    user.Signature = contact["Signature"].ToString();
                    user.Icon = GetIcon(wcs, user.UserName);
                    user.SnsFlag = contact["SnsFlag"].ToString();
                    user.KeyWord = contact["KeyWord"].ToString();
                    user.StartChar = GetStartChar(user);
                    contact_all.Add(user);
                }
            }

            IOrderedEnumerable<object> list_all = contact_all.OrderBy(p => (p as WeChatUser).StartChar).ThenBy(p => (p as WeChatUser).ShowPinYin.Substring(0, 1));

            WeChatUser wcu;
            string start_char;
            foreach (object o in list_all)
            {
                wcu = o as WeChatUser;
                start_char = wcu.StartChar;
                if (!_contact_all.Contains(start_char.ToUpper()))
                {
                    _contact_all.Add(start_char.ToUpper());
                }
                _contact_all.Add(o);
            }
        }
        /// <summary>
        /// 获取头像
        /// </summary>
        /// <param name="wcs"></param>
        /// <param name="_userName"></param>
        /// <returns></returns>
        private ImageSource GetIcon(WeChatService wcs, string _userName)
        {
            if (string.IsNullOrEmpty(_userName))
            {
                return null;
            }
            ImageSource _icon;
            //讨论组
            if (_userName.Contains("@@"))
            {
                _icon = wcs.GetIcon(_userName,StaticUrl.stringWx+ StaticUrl.Url_GetHeadImg);
            }
            //好友
            else if (_userName.Contains("@"))
            {
                _icon = wcs.GetIcon(_userName);
            }
            else
            {
                _icon = wcs.GetIcon(_userName);
            }
            return _icon;
        }
        /// <summary>
        /// 获取分组的头
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string GetStartChar(WeChatUser user)
        {
            string start_char;

            if (user.KeyWord == "gh_" && user.SnsFlag.Equals("0") || user.KeyWord == "cmb")//user.KeyWord =="cmb"是招商银行信用卡，实在找不出区别了
            {
                start_char = "公众号";
            }
            else if (user.UserName.Contains("@@") && user.SnsFlag.Equals("0"))
            {
                start_char = "群聊";
            }
            else
            {
                start_char = string.IsNullOrEmpty(user.ShowPinYin) ? string.Empty : user.ShowPinYin.Substring(0, 1);
            }
            return start_char;
        }

        /// <summary>
        /// 倒计时隐藏按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Tootip_Visibility = Visibility.Collapsed;
            timer.Stop();
        }
        /// <summary>
        /// 获取要发送的Emoji名
        /// </summary>
        /// <param name="str">相对路径的值</param>
        /// <returns></returns>
        private string GetEmojiName(string str)
        {
            foreach (var item in ContantClass.EmojiCode)
            {
                if (item.Value.ToString().Equals(str))
                {
                    return item.Key;
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 将Document里的值都换成String
        /// </summary>
        /// <param name="fld"></param>
        /// <returns></returns>
        private string GetSendMessage(FlowDocument fld)
        {
            if (fld == null)
            {
                return string.Empty;
            }
            string resutStr = string.Empty;
            foreach (var root in fld.Blocks)
            {
                foreach (var item in ((Paragraph)root).Inlines)
                {
                    //如果是Emoji则进行转换
                    if (item is InlineUIContainer)
                    {
                        System.Windows.Controls.Image img = (System.Windows.Controls.Image)((InlineUIContainer)item).Child;
                        resutStr += GetEmojiName(img.Source.ToString());
                    }
                    //如果是文本，则直接赋值
                    if (item is Run)
                    {
                        resutStr += ((Run)item).Text;
                    }
                }
            }
            return resutStr;
        }
        /// <summary>
        /// 获取Emoji的名字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string GetEmojiNameByRegex(string str)
        {
            string name = Regex.Match(str, "(?<=\\[).*?(?=\\])").Value;
            return "[" + name + "]";
        }
        /// <summary>
        /// 获取文本信息
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string GetTextByRegex(string str)
        {
            string text = Regex.Match(str, "^.*?(?=\\[)").Value;
            return text;
        }
        /// <summary>
        /// 将字符串转换成FlowDocument
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <param name="fld">要被赋值的Flowdocument</param>
        /// <param name="par">要添加到Flowdocument里的Paragraph</param>
        private void StrToFlDoc(string str,FlowDocument fld,Paragraph par)
        {
            //当递归结束以后，也就是长度为0的时候，就跳出
            if (str.Length <= 0)
            {
                fld.Blocks.Add(par);
                return;
            }
            //如果字符串里不存在[时，则直接添加内容
            if (!str.Contains('['))
            {
                par.Inlines.Add(new Run(str));
                str = str.Remove(0, str.Length);
                StrToFlDoc(str,fld, par);
            }
            else
            {
                //设置字符串长度
                int strLength = str.Length;
                //首先判断第一位是不是[，如果是，则证明是表情，用正则获取表情，然后将字符串长度进行移除，递归
                if (str[0].Equals('['))
                {
                    par.Inlines.Add(new InlineUIContainer(new System.Windows.Controls.Image { Width = 20, Height = 20, Source = ContantClass.EmojiCode[GetEmojiNameByRegex(str)] }));
                    str = str.Remove(0, GetEmojiNameByRegex(str).Length);
                    StrToFlDoc(str,fld, par);
                }
                else
                {//如果第一位不是[的话，则是字符串，直接添加进去
                    par.Inlines.Add(new Run(GetTextByRegex(str)));
                    str = str.Remove(0, GetTextByRegex(str).Length);
                    StrToFlDoc(str,fld, par);
                }
            }
        }
        #endregion

        #region 聊天事件
        private RelayCommand _loadedCommand;
        /// <summary>
        /// 载入
        /// </summary>
        public RelayCommand LoadedCommand
        {
            get
            {
                return _loadedCommand ?? (_loadedCommand = new RelayCommand(() =>
                    {
                        Thread listener = new Thread(new ThreadStart(new Action(() =>
                        {
                            string sync_flag = "";
                            JObject sync_result;
                            while (true)
                            {
                                //同步检查
                                sync_flag = wcs.WeChatSyncCheck();
                                if (sync_flag == null)
                                {
                                    continue;
                                }
                                //这里应该判断sync_flag中Selector的值
                                else
                                {
                                    sync_result = wcs.WeChatSync();//进行同步
                                    if (sync_result != null)
                                    {
                                        if (sync_result["AddMsgCount"] != null && sync_result["AddMsgCount"].ToString() != "0")
                                        {
                                            foreach (JObject m in sync_result["AddMsgList"])
                                            {
                                                string from = m["FromUserName"].ToString();
                                                string to = m["ToUserName"].ToString();
                                                string content = m["Content"].ToString();
                                                string type = m["MsgType"].ToString();

                                                WeChatMsg msg = new WeChatMsg();
                                                msg.From = from;
                                                msg.Msg = type == "1" ? content : "请在其他设备上查看消息";//只接受文本消息
                                                msg.Readed = false;
                                                msg.Time = DateTime.Now;
                                                msg.To = to;
                                                msg.Type = int.Parse(type);

                                                if (msg.Type == 51)//屏蔽一些系统数据
                                                {
                                                    continue;
                                                }

                                                Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                                                {
                                                    WeChatUser user;
                                                    bool exist_latest_contact = false;
                                                    foreach (object u in Contact_latest)
                                                    {
                                                        user = u as WeChatUser;
                                                        if (user != null)
                                                        {
                                                            //接收别人消息
                                                            if (user.UserName == msg.From && msg.To == _me.UserName)
                                                            {
                                                                Contact_latest.Remove(user);
                                                                user.UnReadCount = user.GetUnReadMsg() == null ? 0 : user.GetUnReadMsg().Count;
                                                                List<WeChatMsg> unReadList = user.GetUnReadMsg();
                                                                WeChatMsg latestMsg = user.GetLatestMsg();
                                                                if (unReadList != null)//未读消息
                                                                {
                                                                    user.LastTime = unReadList[unReadList.Count - 1].Time.ToShortTimeString();
                                                                    //user.LastMsg = unReadList[unReadList.Count - 1].Msg.ToString();
                                                                    //user.LastMsg = user.LastMsg.Length <= 10 ? user.LastMsg : user.LastMsg.Substring(0, 10) + "……";
                                                                    //string str = unReadList[unReadList.Count - 1].Msg.ToString().Length <= 10 ? unReadList[unReadList.Count - 1].Msg : unReadList[unReadList.Count - 1].Msg.Substring(0, 10) + "……";
                                                                    string str = unReadList[unReadList.Count - 1].Msg;
                                                                    StrToFlDoc(str, user.LastMsg = new FlowDocument(), new Paragraph());
                                                                }
                                                                else//最新消息
                                                                {
                                                                    if (latestMsg != null)
                                                                    {
                                                                        user.LastTime = latestMsg.Time.ToShortTimeString();
                                                                        //user.LastMsg = latestMsg.Msg.ToString();
                                                                        //user.LastMsg = user.LastMsg.Length <= 10 ? user.LastMsg : user.LastMsg.Substring(0, 10) + "……";
                                                                        //string str = latestMsg.Msg.ToString().Length <= 10 ? latestMsg.Msg.ToString() : latestMsg.Msg.ToString().Substring(0, 10) + "……";
                                                                        string str = latestMsg.Msg;
                                                                        StrToFlDoc(str, user.LastMsg = new FlowDocument(), new Paragraph());
                                                                    }
                                                                }

                                                                Contact_latest.Insert(0, user);
                                                                exist_latest_contact = true;
                                                                user.ReceivedMsg(msg);
                                                                break;
                                                            }
                                                            //同步自己在其他设备上发送的消息
                                                            else if (user.UserName == msg.To && msg.From == _me.UserName)
                                                            {
                                                                Contact_latest.Remove(user);
                                                                Contact_latest.Insert(0, user);
                                                                exist_latest_contact = true;
                                                                user.SendMsg(msg, true);
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    if (!exist_latest_contact)
                                                    {
                                                        foreach (object o in Contact_all)
                                                        {
                                                            WeChatUser friend = o as WeChatUser;
                                                            if (friend != null && friend.UserName == msg.From && msg.To == _me.UserName)
                                                            {
                                                                Contact_latest.Insert(0, friend);
                                                                friend.ReceivedMsg(msg);
                                                                break;
                                                            }
                                                            if (friend != null && friend.UserName == msg.To && msg.From == _me.UserName)
                                                            {
                                                                Contact_latest.Insert(0, friend);
                                                                friend.SendMsg(msg, true);
                                                                break;
                                                            }
                                                        }
                                                    }
                                                });
                                            }
                                        }
                                    }
                                }
                                Thread.Sleep(10);
                            }
                        })));
                        listener.Start();
                    }));
            }
        }

        private RelayCommand _chatCommand;
        /// <summary>
        /// 聊天列表的选中事件
        /// </summary>
        public RelayCommand ChatCommand
        {
            get
            {
                return _chatCommand ?? (_chatCommand = new RelayCommand(() =>
                    {
                        if (Select_Contact_latest is WeChatUser)
                        {
                            UserName = (Select_Contact_latest as WeChatUser).ShowName;
                            ChatList.Clear();
                            FriendUser = Select_Contact_latest as WeChatUser;
                        }
                    }));
            }
        }

        private RelayCommand _friendCommand;
        /// <summary>
        /// 通讯录选中事件
        /// </summary>
        public RelayCommand FirendCommand
        {
            get
            {
                return _friendCommand ?? (_friendCommand = new RelayCommand(() =>
                    {
                        if (Select_Contact_all is WeChatUser)
                        {
                            FriendInfo = Select_Contact_all as WeChatUser;
                        }
                    }));
            }
        }

        private RelayCommand _friendSendComamnd;
        /// <summary>
        /// 用户信息页面的发送消息按钮
        /// </summary>
        public RelayCommand FriendSendComamnd
        {
            get
            {
                return _friendSendComamnd ?? (_friendSendComamnd = new RelayCommand(() =>
                    {
                        Contact_latest.Remove(Select_Contact_all);
                        Contact_latest.Insert(0, Select_Contact_all);
                        IsChecked = true;
                    }));
            }
        }

        private RelayCommand _sendCommand;
        /// <summary>
        /// 发送消息
        /// </summary>
        public RelayCommand SendCommand
        {
            get
            {
                return _sendCommand ?? (_sendCommand = new RelayCommand(() =>
                    {
                        SendMessage = GetSendMessage(ShowSendMessage);
                        if (!string.IsNullOrEmpty(SendMessage))
                        {
                            WeChatMsg msg = new WeChatMsg();
                            msg.From = _me.UserName;
                            msg.Readed = false;
                            msg.To = _friendUser.UserName;
                            msg.Type = 1;
                            msg.Msg = SendMessage;
                            msg.Time = DateTime.Now;
                            _friendUser.SendMsg(msg, false);
                            SendMessage = string.Empty;
                            ShowSendMessage.Blocks.Clear();
                        }
                        else
                        {
                            Tootip_Visibility = Visibility.Visible;
                            timer.Start();
                        }
                    }));
            }
        }

        /// <summary>
        /// 表示处理开启聊天事件的方法
        /// </summary>
        /// <param name="user"></param>
        public delegate void StartChatEventHandler(WeChatUser user);
        public event StartChatEventHandler StartChat;

        /// <summary>
        /// 发送消息完成
        /// </summary>
        /// <param name="msg"></param>
        void _friendUser_MsgSent(WeChatMsg msg)
        {
            ShowSendMsg(msg);
        }
        /// <summary>
        /// 收到新消息
        /// </summary>
        /// <param name="msg"></param>
        void _friendUser_MsgRecved(WeChatMsg msg)
        {
            ShowReceiveMsg(msg);
        }
        /// <summary>
        /// 显示发出的消息
        /// </summary>
        /// <param name="msg"></param>
        private void ShowSendMsg(WeChatMsg msg)
        {
            try
            {
                ChatMsg chatmsg = new ChatMsg();
                chatmsg.Image = _me.Icon;
                //此处的Paragraph必须是新New的
                Paragraph par = new Paragraph();
                StrToFlDoc(msg.Msg, chatmsg.Message, par);
                chatmsg.FlowDir = FlowDirection.RightToLeft;
                chatmsg.TbColor = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#FF98E165");
                ChatList.Add(chatmsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        /// <summary>
        /// 显示收到的信息
        /// </summary>
        /// <param name="msg"></param>
        private void ShowReceiveMsg(WeChatMsg msg)
        {
            ChatMsg chatmsg = new ChatMsg();
            Contact_all.ForEach(p =>
            {
                if (p is WeChatUser)
                {
                    if ((p as WeChatUser).UserName == msg.From)
                    {
                        chatmsg.Image = (p as WeChatUser).Icon;
                        return;
                    }
                }
            });
            Paragraph par = new Paragraph();
            StrToFlDoc(msg.Msg, chatmsg.Message,par);
            chatmsg.FlowDir = FlowDirection.LeftToRight;
            chatmsg.TbColor = System.Windows.Media.Brushes.White;
            ChatList.Add(chatmsg);
        }
        #endregion

        #region 插件部分按钮事件
        private RelayCommand _emojiCommand;
        /// <summary>
        /// Emoji按钮事件
        /// </summary>
        public RelayCommand EmojiCommand
        {
            get
            {
                return _emojiCommand ?? (_emojiCommand = new RelayCommand(() =>
                    {
                        Emoji_Popup = true;
                    }));
            }
        }
        #endregion
    }
}
