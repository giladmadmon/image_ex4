using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using ImageService.Logging.Modal;

namespace ImageService.GUI.Converters {
    class LogTypeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(targetType != typeof(Brush))
                throw new InvalidOperationException("Must convert to a brush!");
            MessageTypeEnum type = (MessageTypeEnum)value;

            switch(type) {
                case MessageTypeEnum.FAIL:
                    return Brushes.Red;
                case MessageTypeEnum.INFO:
                    return Brushes.Green;
                case MessageTypeEnum.WARNING:
                    return Brushes.Yellow;
            }

            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return DependencyProperty.UnsetValue;
        }
    }
}
