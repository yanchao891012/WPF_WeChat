using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Tools.Helpers
{
    public class TimeHelper
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
            //DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            //long time = (DateTime.Now.Ticks - startTime.Ticks) / 10000;
            //return time;
        }
        /// <summary>
        /// 时间戳取反
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp_TakeBack()
        {
            return ~GetTimeStamp();
        }
    }
}
