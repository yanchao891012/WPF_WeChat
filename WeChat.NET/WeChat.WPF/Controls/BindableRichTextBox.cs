using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WeChat.WPF.Controls
{
    public class BindableRichTextBox : RichTextBox
    {
        public new FlowDocument Document
        {
            get { return (FlowDocument)GetValue(DocumentProperty); }
            set { SetValue(DocumentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Document.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.Register("Document", typeof(FlowDocument), typeof(BindableRichTextBox), new FrameworkPropertyMetadata(null,new PropertyChangedCallback(OnDucumentChanged)));

        private static void OnDucumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RichTextBox rtb = (RichTextBox)d;
            rtb.Document = (FlowDocument)e.NewValue;
        }
    }
}
