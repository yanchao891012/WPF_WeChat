using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.HTTP
{
    public class StaticCode
    {
        public static class LoginCode
        {
            /// <summary>
            /// 确认登录
            /// </summary>
            public static string code_LoginSuccess = "200";
            /// <summary>
            /// 扫描成功
            /// </summary>
            public static string code_LoginWait = "201";
            /// <summary>
            /// 登陆超时
            /// </summary>
            public static string code_LoginTimeOut = "408";
        }

        //public static class MethodCode
        //{
        //    /// <summary>
        //    /// POST
        //    /// </summary>
        //    public static string code_Post = "POST";
        //    /// <summary>
        //    /// GET
        //    /// </summary>
        //    public static string code_Get = "GET";
        //}

        public static class RetCode
        {
            /// <summary>
            /// 正常
            /// </summary>
            public static string code_Normal = "0";
            /// <summary>
            /// 失败/退出微信
            /// </summary>
            public static string code_Miss = "1100";
        }

        public static class SelectorCode
        {
            /// <summary>
            /// 正常
            /// </summary>
            public static string code_Normal = "0";
            /// <summary>
            /// 新的消息
            /// </summary>
            public static string code_New = "2";
            /// <summary>
            /// 进入/离开聊天界面
            /// </summary>
            public static string code_InOrLeave = "7";
        }
    }
}
