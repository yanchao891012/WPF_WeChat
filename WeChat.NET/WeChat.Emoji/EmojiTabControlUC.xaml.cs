using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeChat.Emoji
{
    /// <summary>
    /// EmojiTabControlUC.xaml 的交互逻辑
    /// </summary>
    public partial class EmojiTabControlUC : UserControl
    {
        public EmojiTabControlUC()
        {
            InitializeComponent();
            if (EmojiList.Count>0)
            {
                return;
            }
            AnalysisXML anlyxml = new AnalysisXML();
            anlyxml.AnayXML();
            EmojiList = new ObservableCollection<emojiEntity>(anlyxml.EmojiList);            
        }

        private static ObservableCollection<emojiEntity> emojiList = new ObservableCollection<emojiEntity>();
        /// <summary>
        /// emoji集合
        /// </summary>
        public static ObservableCollection<emojiEntity> EmojiList
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
    }
}
