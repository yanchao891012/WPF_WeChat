using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Media;
using WeChat.Tools.Helpers;

namespace WeChat.HTTP
{
    /// <summary>
    /// 微信主要业务逻辑服务类
    /// </summary>
    public class WeChatService
    {
        private static Dictionary<string, string> _syncKey = new Dictionary<string, string>();
        /// <summary>
        /// 微信初始化
        /// </summary>
        /// <returns></returns>
        public JObject WeChatInit()
        {
            string init_json = "{{\"BaseRequest\":{{\"Uin\":\"{0}\",\"Sid\":\"{1}\",\"Skey\":\"\",\"DeviceID\":\"e" + NumHelper.RandomNum(15) + "\"}}}}";
            Cookie sid = BaseService.GetCookie("wxsid");
            Cookie uin = BaseService.GetCookie("wxuin");
            if (sid != null && uin != null)
            {
                init_json = string.Format(init_json, uin.Value, sid.Value);
                byte[] bytes = BaseService.Request(StaticUrl.stringWx+ StaticUrl.Url_Init + TimeHelper.GetTimeStamp() + "&pass_ticket=" + LoginService.Pass_Ticket, MethodEnum.POST, init_json);
                string init_str = Encoding.UTF8.GetString(bytes);

                JObject init_result = JsonConvert.DeserializeObject(init_str) as JObject;

                foreach (JObject synckey in init_result["SyncKey"]["List"])//同步键值
                {
                    _syncKey.Add(synckey["Key"].ToString(), synckey["Val"].ToString());
                }
                return init_result;
            }
            else
                return null;
        }
        /// <summary>
        /// 获取头像 默认是个人，获取群组时需要传入URL
        /// </summary>
        /// <param name="username">用户名或群组名</param>
        /// <param name="url">个人或群组URL</param>
        /// <returns></returns>
        public ImageSource GetIcon(string username, string url = "")
        {
            if (string.IsNullOrEmpty(url))
            {
                url =StaticUrl.stringWx+ StaticUrl.Url_GetIcon;
            }
            byte[] bytes = BaseService.Request(url + username, MethodEnum.GET);
            return ImageHelper.MemoryToImageSourceOther(new MemoryStream(bytes));
        }
        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <returns></returns>
        public JObject GetContact()
        {
            byte[] bytes = BaseService.Request(StaticUrl.stringWx+ StaticUrl.Url_GetContact, MethodEnum.GET);
            string contact_str = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject(contact_str) as JObject;
        }
        /// <summary>
        /// 微信同步检测
        /// </summary>
        /// <returns></returns>
        public string WeChatSyncCheck()
        {
            string sync_key = "";
            foreach (KeyValuePair<string, string> p in _syncKey)
            {
                sync_key += p.Key + "_" + p.Value + "%7C";
            }
            sync_key = sync_key.TrimEnd('%', '7', 'C');

            Cookie sid = BaseService.GetCookie("wxsid");
            Cookie uin = BaseService.GetCookie("wxuin");

            if (sid != null && uin != null)
            {
                StaticUrl.Url_SyncCheck_ext = string.Format(StaticUrl.Url_SyncCheck_ext, sid.Value, uin.Value, sync_key, TimeHelper.GetTimeStamp(), LoginService.SKey.Replace("@", "%40"), "e" + NumHelper.RandomNum(15));
                byte[] bytes = BaseService.Request(StaticUrl.stringWebPush+ StaticUrl.Url_SyncCheck + StaticUrl.Url_SyncCheck_ext + DateTime.Now.Ticks, MethodEnum.GET);
                if (bytes != null)
                {
                    return Encoding.UTF8.GetString(bytes);
                }
                else
                    return null;
            }
            else
                return null;
        }
        /// <summary>
        /// 微信同步
        /// </summary>
        /// <returns></returns>
        public JObject WeChatSync()
        {
            string sync_json = "{{\"BaseRequest\" : {{\"DeviceID\":\"e" + NumHelper.RandomNum(15) + "\",\"Sid\":\"{1}\", \"Skey\":\"{5}\", \"Uin\":\"{0}\"}},\"SyncKey\" : {{\"Count\":{2},\"List\":[{3}]}},\"rr\" :{4}}}";
            Cookie sid = BaseService.GetCookie("wxsid");
            Cookie uin = BaseService.GetCookie("wxuin");

            string sync_keys = "";
            foreach (KeyValuePair<string, string> p in _syncKey)
            {
                sync_keys += "{\"Key\":" + p.Key + ",\"Val\":" + p.Value + "},";
            }
            sync_keys = sync_keys.TrimEnd(',');
            sync_json = string.Format(sync_json, uin.Value, sid.Value, _syncKey.Count, sync_keys, TimeHelper.GetTimeStamp(), LoginService.SKey);

            if (sid != null && uin != null)
            {
                StaticUrl.Url_Sync_ext = string.Format(StaticUrl.Url_Sync_ext, sid.Value, LoginService.SKey, LoginService.Pass_Ticket);
                byte[] bytes = BaseService.Request(StaticUrl.stringWx+ StaticUrl.Url_Sync + StaticUrl.Url_Sync_ext, MethodEnum.POST, sync_json);
                string sync_str = Encoding.UTF8.GetString(bytes);

                JObject sync_result = JsonConvert.DeserializeObject(sync_str) as JObject;
                if (sync_result["SyncKey"]["Count"].ToString() != "0")
                {
                    _syncKey.Clear();
                    foreach (JObject key in sync_result["SyncKey"]["List"])
                    {
                        _syncKey.Add(key["Key"].ToString(), key["Val"].ToString());
                    }
                }
                return sync_result;
            }
            else
                return null;
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg">发送内容</param>
        /// <param name="from">发自</param>
        /// <param name="to">发给</param>
        /// <param name="type">类型</param>
        public void SendMsg(string msg, string from, string to, int type)
        {
            string msg_json = "{{" +
            "\"BaseRequest\":{{" +
                "\"DeviceID\" : \"e" + NumHelper.RandomNum(15) + "\"," +
                "\"Sid\" : \"{0}\"," +
                "\"Skey\" : \"{6}\"," +
                "\"Uin\" : \"{1}\"" +
            "}}," +
            "\"Msg\" : {{" +
                "\"ClientMsgId\" : {8}," +
                "\"Content\" : \"{2}\"," +
                "\"FromUserName\" : \"{3}\"," +
                "\"LocalID\" : {9}," +
                "\"ToUserName\" : \"{4}\"," +
                "\"Type\" : {5}" +
            "}}," +
            "\"rr\" : {7}" +
            "}}";

            Cookie sid = BaseService.GetCookie("wxsid");
            Cookie uin = BaseService.GetCookie("wxuin");

            if (sid != null && uin != null)
            {
                msg_json = string.Format(msg_json, sid.Value, uin.Value, msg, from, to, type, LoginService.SKey, DateTime.Now.Millisecond, DateTime.Now.Millisecond, DateTime.Now.Millisecond);
                byte[] bytes = BaseService.Request(StaticUrl.stringWx+ StaticUrl.Url_SendMsg + sid.Value + "&pass_ticket=" + LoginService.Pass_Ticket, MethodEnum.POST, msg_json);
                string send_result = Encoding.UTF8.GetString(bytes);
            }
        }
    }
}
