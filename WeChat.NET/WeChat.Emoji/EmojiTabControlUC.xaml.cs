using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public event EventHandler Close;
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

        private KeyValuePair<string, BitmapImage> selectEmoji = new KeyValuePair<string, BitmapImage>();
        /// <summary>
        /// 选中项
        /// </summary>
        public KeyValuePair<string, BitmapImage> SelectEmoji
        {
            get
            {
                return selectEmoji;
            }

            set
            {
                selectEmoji = value;
            }
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
        /// <summary>
        /// 点选事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (lb.SelectedItem != null)
            {
                SelectEmoji = (KeyValuePair<string, BitmapImage>)lb.SelectedItem;
                if (Close != null)
                {
                    Close(this, null);
                }
            }
            else
                return;
        }
    }
}
