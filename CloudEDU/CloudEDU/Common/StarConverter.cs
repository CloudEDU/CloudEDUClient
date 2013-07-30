using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace CloudEDU.Common
{
    /// <summary>
    /// Convert star text property.
    /// </summary>
    class StarTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int starNum = System.Convert.ToInt32(parameter as string);
            double rate = (double)value;

            if (starNum <= rate + 1)
            {
                return Constants.FillStar;
            }
            else
            {
                return Constants.BlankStar;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Convert star widtg property.
    /// </summary>
    class StarWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int starNum = System.Convert.ToInt32(parameter as string);
            double rate = (double)value;

            if (starNum == (int)rate + 1)
            {
                double width = (rate - (double)starNum + 1) * Constants.StarWidth;
                return width;
            }
            else
            {
                return Constants.StarWidth;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Convert star margin property.
    /// </summary>
    class StarMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int starNum = System.Convert.ToInt32(parameter as string);
            double rate = (double)value;

            if (starNum == (int)rate + 2)
            {
                double left = -((rate - (double)starNum + 2) * Constants.StarWidth);
                return new Thickness(left, 0, 0, 0);
            }
            else
            {
                return new Thickness(0);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
