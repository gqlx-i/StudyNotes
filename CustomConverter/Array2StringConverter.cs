using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StudyNotes.CustomConverter
{
    public class Array2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
       {
            StringBuilder builder = new StringBuilder();
            if (value == null) { return ""; }
            try
            {
                object[] arr = (object[])value;
                builder.Append("[");
                foreach (var item in arr)
                {
                    builder.Append(item.ToString());
                    builder.Append(", ");
                }
                if (builder.Length > 2)
                {
                    builder.Remove(builder.Length - 2, 2);
                }
                builder.Append("]");
            }
            catch (Exception ex)
            {

            }
            return builder.ToString(); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
