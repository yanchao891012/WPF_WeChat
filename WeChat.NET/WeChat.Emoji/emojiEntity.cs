using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace WeChat.Emoji
{
    public class emojiEntity
    {
        private string _key;//分组
        private BitmapImage _keyImg;//分组图像
        private Dictionary<string, BitmapImage> _emojiCode = new Dictionary<string, BitmapImage>();//emoji编码

        /// <summary>
        /// 分组
        /// </summary>
        public string Key
        {
            get
            {
                return _key;
            }

            set
            {
                _key = value;
            }
        }
        /// <summary>
        /// emoji编码
        /// </summary>
        public Dictionary<string, BitmapImage> EmojiCode
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
        /// <summary>
        /// 分组图像
        /// </summary>
        public BitmapImage KeyImg
        {
            get
            {
                return _keyImg = EmojiCode.Values.First();
            }
        }
    }
}
