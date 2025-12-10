using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace TonProjet.Converters // Remplace TonProjet par ton namespace
{
    public class TextTransformConverter : IValueConverter
    {
        public static readonly TextTransformConverter Instance = new();

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string input)
            {
                if (parameter is string transformType)
                {
                    return transformType.ToLowerInvariant() switch
                    {
                        "uppercase" => input.ToUpperInvariant(),
                        "lowercase" => input.ToLowerInvariant(),
                        "titlecase" => culture.TextInfo.ToTitleCase(input),
                        _ => input
                    };
                }
                return input;
            }
            return value!;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}