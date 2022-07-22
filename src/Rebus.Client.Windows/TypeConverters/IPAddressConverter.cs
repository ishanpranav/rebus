// Ishan Pranav's REBUS: IPAddressConverter.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Net;
using System.Reflection;

namespace Rebus.Client.Windows.TypeConverters
{
    internal sealed class IPAddressConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string ipString)
            {
                return IPAddress.Parse(ipString);
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor) && value is IPAddress ipAddress)
            {
                ConstructorInfo? constructor = typeof(IPAddress).GetConstructor(new Type[]
                {
                    typeof(string)
                });

                if (constructor is not null)
                {
                    return new InstanceDescriptor(constructor, new object[]
                    {
                        ipAddress.ToString()
                    });
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
