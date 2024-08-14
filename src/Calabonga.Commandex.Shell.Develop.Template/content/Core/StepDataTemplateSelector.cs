using System.Globalization;
using System.Runtime.Serialization;
using System.Windows.Data;
using System.Windows.Media;
using Calabonga.Commandex.Engine.Wizards;

namespace Calabonga.Commandex.Shell.Develop.Core;

/// <summary>
/// // Calabonga: Summary required (StepDataTemplateSelector 2024-08-12 03:05)
/// </summary>
[KnownType(typeof(IWizardStep))]
public class IsActiveColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool active)
        {
            if (active)
            {
                return new SolidColorBrush(Colors.Teal);
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        throw new NotImplementedException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}