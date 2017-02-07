using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WeChat.Tools.Helpers;

namespace WeChat.HTTP
{
    public class LoginService
    {
        public static string Pass_Ticket = "";
        public static string SKey = "";
        private static string session_id = null;

        /// <summary>
        /// 获取登录的二维码
        /// </summary>
        /// <returns></returns>
        public ImageSource GetQRCode()
        {
            //请求UUID
            byte[] bytes = BaseService.Request(StaticUrl.Url_GetUUID + TimeHelper.GetTimeStamp(), MethodEnum.GET);
            //得到session
            session_id = Encoding.UTF8.GetString(bytes).Split(new string[] { "\"" }, StringSplitOptions.None)[1];
            //请求二维码
            bytes = BaseService.Request(StaticUrl.Url_GetQrCode + session_id, MethodEnum.GET);
            //转换成图片
            return ImageHelper.MemoryToImageSource_Transparent(new MemoryStream(bytes));
        }
        /// <summary>
        /// 登录扫描检测
        /// </summary>
        /// <returns></returns>
        public object LoginCheck()
        {
            if (session_id == null)
                return null;
            //查看是否扫码登录了
            byte[] bytes = BaseService.Request(StaticUrl.Url_WaitLogin + session_id + "&tip=0&r=" + TimeHelper.GetTimeStamp_TakeBack() + "&_=" + TimeHelper.GetTimeStamp(), MethodEnum.GET);

            string login_result = Encoding.UTF8.GetString(bytes);

            if (login_result.Contains("=" + StaticCode.LoginCode.code_LoginSuccess))
            {
                string login_redirect_url = login_result.Split(new string[] { "\"" }, StringSplitOptions.None)[1];
                string string_url_front = login_redirect_url.Split(new string[] { "?" }, StringSplitOptions.None)[0];
                if (string_url_front.IndexOf("wx2.qq.com")>-1)
                {
                    StaticUrl.stringWx = "https://wx2.qq.com";
                    StaticUrl.stringWebPush = "https://webpush2.weixin.qq.com";
                }
                if (string_url_front.IndexOf("wx.qq.com") >-1)
                {
                    StaticUrl.stringWx = "https://wx.qq.com";
                    StaticUrl.stringWebPush = "https://webpush.weixin.qq.com";
                }
                if (string_url_front.IndexOf("web1.wechat.com") > -1)
                {
                    StaticUrl.stringWx = "https://web.wechat.com";
                    StaticUrl.stringWebPush = "https://webpush1.wechat.com";
                }
                if (string_url_front.IndexOf("web2.wechat.com") > -1)
                {
                    StaticUrl.stringWx = "https://web.wechat.com";
                    StaticUrl.stringWebPush = "https://webpush2.wechat.com";
                }
                if (string_url_front.IndexOf("web.wechat.com") > -1)
                {
                    StaticUrl.stringWx = "https://web.wechat.com";
                    StaticUrl.stringWebPush = "https://webpush.wechat.com";
                }
                if (string_url_front.IndexOf("web1.wechatapp.com") > -1)
                {
                    StaticUrl.stringWx = "https://web.wechatapp.com";
                    StaticUrl.stringWebPush = "https://webpush1.wechatapp.com";
                }
                if (string_url_front.IndexOf("web.wechatapp.com") > -1)
                {
                    StaticUrl.stringWx = "https://web.wechatapp.com";
                    StaticUrl.stringWebPush = "https://webpush.wechatapp.com";
                }
                return login_redirect_url;
            }
            else if (login_result.Contains("=" + StaticCode.LoginCode.code_LoginWait))
            {
                string base64_image = login_result.Split(new string[] { "\'" }, StringSplitOptions.None)[1].Split(',')[1];
                byte[] base64_image_bytes = Convert.FromBase64String(base64_image);
                MemoryStream memoryStream = new MemoryStream(base64_image_bytes, 0, base64_image_bytes.Length);
                //memoryStream.Write(base64_image_bytes, 0, base64_image_bytes.Length);
                //转成图片
                return ImageHelper.MemoryToImageSource(memoryStream);
            }
            //注：如果用超时的话，会有问题，后期再研究
            //else if (login_result.Contains("=" + StaticCode.LoginCode.code_LoginTimeOut))
            //{
            //    session_id = null;
            //    return 408;
            //}
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取sid uid 结果放在cookie中
        /// </summary>
        /// <param name="login_redirect"></param>
        public void GetSidUid(string login_redirect)
        {
            byte[] bytes = BaseService.Request(login_redirect + StaticUrl.Url_redirect_ext, MethodEnum.GET);
            string pass_ticket = Encoding.UTF8.GetString(bytes);
            Pass_Ticket = pass_ticket.Split(new string[] { "pass_ticket" }, StringSplitOptions.None)[1].TrimStart('>').TrimEnd('<', '/');
            SKey = pass_ticket.Split(new string[] { "skey" }, StringSplitOptions.None)[1].TrimStart('>').TrimEnd('<', '/');
        }
    }
}
