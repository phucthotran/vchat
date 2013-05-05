using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace vChat.Lib
{
    public class ImageByteConverter
    {
        public static ImageSource ResizeImageByWidth(byte[] ImageData, int SourceWidth, int SourceHeight, int NewWidth)
        {
            if (ImageData == null)
                return null;

            float resizePercent = ((float)(NewWidth * 100) / (float)SourceWidth);

            int resizeWidth = SourceWidth - (((int)(SourceWidth * (100.0 - resizePercent))) / 100);
            int resizeHeight = SourceHeight - (((int)(SourceHeight * (100.0 - resizePercent))) / 100);

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.DecodePixelWidth = resizeWidth;
            bitmap.DecodePixelHeight = resizeHeight;
            bitmap.StreamSource = new MemoryStream(ImageData);
            bitmap.CreateOptions = BitmapCreateOptions.None;
            bitmap.CacheOption = BitmapCacheOption.Default;
            bitmap.EndInit();

            return bitmap as ImageSource;
        }

        public static ImageSource GetFromBytes(byte[] ImageData)
        {
            if (ImageData == null)
                return null;

            MemoryStream msImage = new MemoryStream(ImageData);
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = msImage;
            bitmap.EndInit();

            return bitmap as ImageSource;
        }

        public static byte[] GetFromFile(String Path)
        {
            byte[] imageData = null;

            using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    imageData = new byte[(int)fs.Length];
                    br.Read(imageData, 0, (int)fs.Length);
                    br.Close();                    
                }
                fs.Close();
            }

            return imageData;
        }

        public static void SaveImage(byte[] ImageBytes, String Path)
        {
            using (FileStream fs = new FileStream(Path, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(ImageBytes);
                    bw.Close();                    
                }
                fs.Close();
            }
        }

        public static byte[] GetEncodedImageData(ImageSource Image, string PreferredFormat)
        {
            byte[] result = null;
            BitmapEncoder encoder = null;

            switch (PreferredFormat.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    encoder = new JpegBitmapEncoder();
                    break;

                case ".bmp":
                    encoder = new BmpBitmapEncoder();
                    break;

                case ".png":
                    encoder = new PngBitmapEncoder();
                    break;

                case ".gif":
                    encoder = new GifBitmapEncoder();
                    break;
            }

            if (Image is BitmapSource)
            {
                MemoryStream stream = new MemoryStream();
                encoder.Frames.Add(BitmapFrame.Create(Image as BitmapSource));
                encoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);
                result = new byte[stream.Length];
                BinaryReader br = new BinaryReader(stream);
                br.Read(result, 0, (int)stream.Length);
                br.Close();
                stream.Close();
            }

            return result;
        }
    }
}
