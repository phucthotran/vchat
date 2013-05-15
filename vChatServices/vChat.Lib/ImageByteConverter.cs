using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace vChat.Lib
{
    /// <summary>
    /// Chuyển đổi ImageSource thành byte và ngược lại. Ngoài ra còn hỗ trợ chuyển đổi file ảnh sang dữ liệu byte và lưu dữ liệu byte thành file ảnh
    /// </summary>
    public class ImageByteConverter
    {
        /// <summary>
        /// Thay đổi kích thước ảnh theo chiều dài
        /// </summary>
        /// <param name="ImageData">Dữ liệu ảnh</param>
        /// <param name="SourceWidth">Chiều dài ban đầu</param>
        /// <param name="SourceHeight">Chiều cao ban đầu</param>
        /// <param name="NewWidth">Chiều dài mới</param>
        /// <returns>Đối tượng ImageSource</returns>
        public static ImageSource ResizeImageByWidth(byte[] ImageData, int SourceWidth, int SourceHeight, int NewWidth)
        {
            if (ImageData == null)
                return null;

            float resizePercent = ((float)(NewWidth * 100) / (float)SourceWidth); //Lấy ra tỉ lệ sau khi resize (theo chiều dài)

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

        /// <summary>
        /// Lấy ra đối tượng ImageSource từ dữ liệu ảnh dưới dạng byte
        /// </summary>
        /// <param name="ImageData">Dữ liệu ảnh</param>
        /// <returns>Đối tượng ImageSource</returns>
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

        /// <summary>
        /// Đọc dữ liệu ảnh từ file
        /// </summary>
        /// <param name="Path">Đường dẫn đến file ảnh</param>
        /// <returns>Mảng dữ liệu byte</returns>
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

        /// <summary>
        /// Lưu dữ liệu ảnh thành file ảnh
        /// </summary>
        /// <param name="ImageBytes">Dữ liệu ảnh</param>
        /// <param name="Path">Đường dẫn file ảnh</param>        
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

        /// <summary>
        /// Lấy ra dữ liệu ảnh trên ImageSource
        /// </summary>
        /// <param name="Image">Đối tượng ImageSource</param>
        /// <param name="PreferredFormat">Định dạng ảnh</param>
        /// <returns>Mảng dữ liệu byte</returns>
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
