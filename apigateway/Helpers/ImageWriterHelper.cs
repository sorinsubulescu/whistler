using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace apigateway
{
    public class ImageWriterHelper
    {
        public enum ImageFormat
        {
            Bmp,
            Jpeg,
            Gif,
            Tiff,
            Png,
            Unknown
        }

        public static ImageFormat GetImageFormat(IEnumerable<byte> bytes)
        {
            var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };              // PNG
            var tiff = new byte[] { 73, 73, 42 };                  // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };                 // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 };          // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 };         // jpeg canon

            var byteArray = bytes as byte[] ?? bytes.ToArray();

            if (bmp.SequenceEqual(byteArray.Take(bmp.Length)))
                return ImageFormat.Bmp;

            if (gif.SequenceEqual(byteArray.Take(gif.Length)))
                return ImageFormat.Gif;

            if (png.SequenceEqual(byteArray.Take(png.Length)))
                return ImageFormat.Png;

            if (tiff.SequenceEqual(byteArray.Take(tiff.Length)))
                return ImageFormat.Tiff;

            if (tiff2.SequenceEqual(byteArray.Take(tiff2.Length)))
                return ImageFormat.Tiff;

            if (jpeg.SequenceEqual(byteArray.Take(jpeg.Length)))
                return ImageFormat.Jpeg;

            if (jpeg2.SequenceEqual(byteArray.Take(jpeg2.Length)))
                return ImageFormat.Jpeg;

            return ImageFormat.Unknown;
        }
    }
}