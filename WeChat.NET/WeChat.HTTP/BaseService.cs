using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.HTTP
{
    /// <summary>
    /// 访问http服务器类
    /// </summary>
    public class BaseService
    {
        /// <summary>
        /// 访问服务器时的cookies
        /// </summary>
        public static CookieContainer CookiesContainer;
        /// <summary>
        /// 向服务器发送Request
        /// </summary>
        /// <param name="url">字符串</param>
        /// <param name="method">枚举类型的方法Get或者Post</param>
        /// <param name="body">Post时必须传值</param>
        /// <returns></returns>
        public static byte[] Request(string url, MethodEnum method, string body = "")
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method.ToString();
                //如果是Post的话，则设置body
                if (method == MethodEnum.POST)
                {
                    byte[] request_body = Encoding.UTF8.GetBytes(body);
                    request.ContentLength = request_body.Length;

                    Stream request_stream = request.GetRequestStream();
                    request_stream.Write(request_body, 0, request_body.Length);
                }
                if (CookiesContainer == null)
                {
                    CookiesContainer = new CookieContainer();
                }
                //启用Cookie
                request.CookieContainer = CookiesContainer;

                return Response(request);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 返回Response数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static byte[] Response(HttpWebRequest request)
        {
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream response_stream = response.GetResponseStream();
                //获取返回报文内容长度
                int count = (int)response.ContentLength;
                int offset = 0;
                byte[] buf = new byte[count];
                //读取返回数据
                while (count > 0)
                {
                    int n = response_stream.Read(buf, offset, count);
                    if (n == 0)
                    {
                        break;
                    }
                    count -= n;
                    offset += n;
                }
                return buf;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }

        /// <summary>
        /// 获取所有的Cookie
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        private static List<Cookie> GetAllCookies(CookieContainer cc)
        {
            List<Cookie> lstCookies = new List<Cookie>();
            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, cc, new object[] { });

            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                {
                    foreach (Cookie c in colCookies)
                    {
                        lstCookies.Add(c);
                    }
                }
            }

            return lstCookies;
        }

        /// <summary>
        /// 获取指定cookie
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Cookie GetCookie(string name)
        {
            List<Cookie> cookies = GetAllCookies(CookiesContainer);
            foreach (Cookie c in cookies)
            {
                if (c.Name == name)
                {
                    return c;
                }
            }
            return null;
        }
    }
}
