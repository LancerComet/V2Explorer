using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace V2EX.Converter {
  /// <summary>
  /// True => Visible
  /// </summary>
  public class TrueToVisible: IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) {
      return (bool)value ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack (object value, Type targetType, object parameter, string language) {
      return (Visibility)value == Visibility.Visible;
    }
  }

  /// <summary>
  /// False => Visible
  /// </summary>
  public class FalseToVisible : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) {
      return (bool)value ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) {
      return (Visibility)value != Visibility.Visible;
    }
  }

  /// <summary>
  /// String => Uri
  /// </summary>
  public class StringToUri : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) {
      return new Uri((string)value, UriKind.Absolute);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) {
      return "";
    }
  }
}
