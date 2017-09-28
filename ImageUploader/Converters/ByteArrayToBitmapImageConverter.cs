using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ImageUploader.Converters
{
    /// <summary>
    /// Конвертер, преобразующий массив байт в картинку
    /// </summary>
    internal sealed class ByteArrayToBitmapImageConverter : IValueConverter
    {
        /// <summary>
        /// Метод конвертации массива байт в картинку
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] imageData = (byte[])value;
            if (imageData == null || imageData.Length == 0) return null;
            BitmapImage image = new BitmapImage();
            using (var stream = new MemoryStream(imageData))
            {
                stream.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = stream;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
