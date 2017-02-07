using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WeChat.Tools.Helpers
{
    public class ImageHelper
    {
        /// <summary>
        /// Memory转成ImageSource
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static ImageSource MemoryToImageSource(MemoryStream ms)
        {
            return (ImageSource)(new ImageSourceConverter()).ConvertFrom(ms);
        }
        /// <summary>
        /// Memory转成ImageSource
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static ImageSource MemoryToImageSourceOther(MemoryStream ms)
        {
            Image img = Image.FromStream(ms);
            Bitmap bmp = new Bitmap(img);
            BitmapSource bi = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            return bi;
        }
        /// <summary>
        /// Memory转成ImageSource
        /// 二维码的白色部分变成透明
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static ImageSource MemoryToImageSource_Transparent(MemoryStream ms)
        {
            Image img = Image.FromStream(ms);
            Bitmap bmp = new Bitmap(img);
            bmp.MakeTransparent(System.Drawing.Color.White);
            BitmapSource bi= System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            return bi;
        }
    }
}
