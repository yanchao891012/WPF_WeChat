using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace WeChat.Emoji
{
    public class ContantClass
    {
        private static Dictionary<string, BitmapImage> _emojiCode = new Dictionary<string, BitmapImage>();//emoji编码
        /// <summary>
        /// emoji编码
        /// </summary>
        public static Dictionary<string, BitmapImage> EmojiCode
        {
            get
            {
                return _emojiCode;
            }

            set
            {
                _emojiCode = value;
            }
        }
    }
}
