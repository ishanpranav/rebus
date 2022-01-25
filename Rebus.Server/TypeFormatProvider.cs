// Ishan Pranav's REBUS: TypeFormatProvider.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Globalization;
using System.Text;

namespace Rebus.Server
{
    internal sealed class TypeFormatProvider : ICustomFormatter, IFormatProvider
    {
        public object? GetFormat(Type? formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        private void Format(Type value, StringBuilder stringBuilder)
        {
            string name = value.Name;
            int start = 0;

            if (value.IsInterface && name.StartsWith('I'))
            {
                start = 1;
            }

            if (value.IsGenericType)
            {
                foreach (Type genericArgument in value.GetGenericArguments())
                {
                    Format(genericArgument, stringBuilder);
                }

                stringBuilder.Append(name.AsSpan(start, name.IndexOf('`') - start));
            }
            else
            {
                stringBuilder.Append(name);
            }
        }

        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            if (arg is Type type)
            {
                switch (format)
                {
                    case "c":
                    case "C":
                        {
                            StringBuilder result = new StringBuilder(type.Namespace)
                                .Append('.');

                            Format(type, result);

                            return result
                                .Append("Collection")
                                .ToString();
                        }

                    case "f":
                    case "F":
                        {
                            StringBuilder result = new StringBuilder(type.Namespace)
                                .Append('.');

                            Format(type, result);

                            return result.ToString();
                        }

                    default:
                        return type.ToString();
                }
            }
            else if (arg is IFormattable formattable)
            {
                return formattable.ToString(format, CultureInfo.CurrentCulture);
            }
            else
            {
                return arg?.ToString() ?? string.Empty;
            }
        }
    }
}
