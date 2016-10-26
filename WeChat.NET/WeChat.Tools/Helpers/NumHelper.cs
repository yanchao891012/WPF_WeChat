using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeChat.Tools.Helpers
{
    public class NumHelper
    {
        /// <summary>
        /// 返回一个随机数
        /// </summary>
        /// <param name="N">位数</param>
        /// <returns></returns>
        public static string RandomNum(int N)
        {
            string resultNum = string.Empty;
            Random random = new Random();
            for (int i = 0; i < N; i++)
            {
                resultNum += random.Next(9);
            }
            return resultNum;
        }
    }
}
