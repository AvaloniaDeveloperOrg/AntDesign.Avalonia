﻿using AntDesign.Enums;

namespace AntDesign.Converters;
public class BorderThicknessFilterConverter : IValueConverter
{
    public BorderEdges Edges { get; set; }

    public double Scale { get; set; } = 1.0;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Thickness thickness)
            return value;

        return new Thickness(Edges.HasFlag(BorderEdges.Left) ? (thickness.Left) * Scale : 0d,
                             Edges.HasFlag(BorderEdges.Top) ? (thickness.Top) * Scale : 0d,
                             Edges.HasFlag(BorderEdges.Right) ? (thickness.Right) * Scale : 0d,
                             Edges.HasFlag(BorderEdges.Bottom) ? (thickness.Bottom) * Scale : 0d);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
