using System.ComponentModel;
using System.Text.RegularExpressions;

namespace ImageUploader.Models
{
    internal sealed class BitmapWrapper : IDataErrorInfo
    {
        public string URL;

        public byte[] ImageData;

        #region IDataErrorInfo implementation
        public string this[string columnName]
        {
            get
            {
                string errorMessage = null;
                switch (columnName)
                {
                    case "URL":
                        if (string.IsNullOrWhiteSpace(URL))
                        {
                            errorMessage = @"Поле ""URL"" не может быть пустым";
                            break;
                        }
                        if (!Regex.IsMatch(URL, @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$"))
                        {
                            errorMessage = @"Введенная строка не является URL";
                            break;
                        }
                        break;

                }
                return errorMessage;
            }
        }

        public string Error => this["URL"];
        #endregion
    }
}
