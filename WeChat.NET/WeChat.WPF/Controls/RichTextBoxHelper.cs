using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WeChat.WPF.Controls
{
    class RichTextBoxHelper : DependencyObject
    {
        public static string GetDocumentXaml(DependencyObject obj)
        {
            return (string)obj.GetValue(DocumentXamlProperty);
        }

        public static void SetDocumentXaml(DependencyObject obj, string value)
        {
            obj.SetValue(DocumentXamlProperty, value);
        }

        // Using a DependencyProperty as the backing store for DocumentXaml.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DocumentXamlProperty =
            DependencyProperty.RegisterAttached("DocumentXaml", typeof(string), typeof(RichTextBoxHelper), new FrameworkPropertyMetadata {
                BindsTwoWayByDefault=true,
                PropertyChangedCallback=(obj,e)=>
                {
                    var richTextBox = (RichTextBox)obj;
                    //Parse the XAML to a document
                    var xaml = GetDocumentXaml(richTextBox);
                    var doc = new FlowDocument();
                    var range = new TextRange(doc.ContentStart, doc.ContentEnd);
                    //Set the document
                    richTextBox.Document = doc;
                    //When the document changes update the source
                    range.Changed += (obj2, e2) =>
                      {
                          if (richTextBox.Document == doc)
                          {
                              MemoryStream buffer = new MemoryStream();
                              range.Save(buffer, DataFormats.Xaml);
                              SetDocumentXaml(richTextBox, Encoding.UTF8.GetString(buffer.ToArray()));
                          }
                      };
                }
            });


    }
}
