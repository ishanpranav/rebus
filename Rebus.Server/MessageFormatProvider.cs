// Ishan Pranav's REBUS: MessageFormatProvider.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;

namespace Rebus.Server
{
    internal class MessageFormatProvider : ICustomFormatter, IFormatProvider
    {
        private readonly ExpressionWriter _writer;

        public MessageFormatProvider(ExpressionWriter writer)
        {
            _writer = writer;
        }

        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            ExpressionWriter writer = _writer.CreateFragment();

            writer.Write(arg, format);

            return writer.ToString();
        }

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
    }
}
