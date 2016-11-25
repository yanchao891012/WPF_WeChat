using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Media.Imaging;
using System.Xml;

namespace WeChat.Emoji
{
    public class AnalysisXML
    {
        private List<emojiEntity> emojiList = new List<emojiEntity>();
        /// <summary>
        /// emoji集合
        /// </summary>
        public List<emojiEntity> EmojiList
        {
            get
            {
                return emojiList;
            }

            set
            {
                emojiList = value;
            }
        }
        /// <summary>
        /// 解析xml
        /// </summary>
        public void AnayXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            Assembly _assembly = Assembly.GetExecutingAssembly();
            Stream _stream = _assembly.GetManifestResourceStream("WeChat.Emoji.Emoji.xml");//文件需为嵌入的资源
            xmlDoc.Load(_stream);
            XmlNode root = xmlDoc.SelectSingleNode("array");
            XmlNodeList nodeList = root.ChildNodes;
            //循环列表，获得相应的内容
            foreach (XmlNode xn in nodeList)
            {
                XmlElement xe = (XmlElement)xn;
                XmlNodeList subList = xe.ChildNodes;
                emojiEntity entity = new emojiEntity();
                foreach (XmlNode xmlNode in subList)
                {
                    if (xmlNode.Name == "key")
                    {
                        entity.Key = xmlNode.InnerText;
                    }
                    if (xmlNode.Name == "array")
                    {
                        XmlElement lastXe = (XmlElement)xmlNode;
                        foreach (XmlNode lastNode in lastXe)
                        {
                            if (lastNode.Name == "a")
                            {
                                entity.EmojiCode.Add(GetEmojiStr(lastNode.InnerText), GetEmojiImage(lastNode.Attributes[1].Value));
                            }
                        }
                    }
                }
                EmojiList.Add(entity);
            }
            foreach (var item in EmojiList)
            {
                //所有的内容都添加到一个dictionary中
                ContantClass.EmojiCode=ContantClass.EmojiCode.Concat(item.EmojiCode).ToDictionary(k=>k.Key,v=>v.Value);
            }
        }
        /// <summary>
        /// 返回Emoji字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetEmojiStr(string name)
        {
            return "[" + name + "]";
        }
        /// <summary>
        /// 返回Emoji图像
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private BitmapImage GetEmojiImage(string name)
        {
            BitmapImage bitmap = new BitmapImage();
            string imgUrl = "pack://application:,,,/WeChat.Emoji;component/Image/" + name + ".png";
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imgUrl, UriKind.RelativeOrAbsolute);
            bitmap.EndInit();
            return bitmap;
        }
    }
}
