using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
            xmlDoc.Load(Path.Combine(Environment.CurrentDirectory, @"Emoji.xml"));
            XmlNode root = xmlDoc.SelectSingleNode("array");
            XmlNodeList nodeList = root.ChildNodes;
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
                            if (lastNode.Name == "e")
                            {
                                entity.EmojiCode.Add(lastNode.InnerText, GetEmojiPath(lastNode.InnerText));
                            }
                        }
                    }
                }
                EmojiList.Add(entity);
            }
        }
        /// <summary>
        /// 返回Emoji路径
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetEmojiPath(string name)
        {
            return "../image/" + "emoji_" + name + ".png";
        }
    }
}
