// Ishan Pranav's REBUS: ExpressionFormatProvider.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Rebus
{
    /// <summary>
    /// Supports custom formatting of objects using an <see cref="ExpressionWriter"/>.
    /// </summary>
    public class ExpressionFormatProvider : ICustomFormatter, IFormatProvider
    {
        /// <inheritdoc/>
        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            ExpressionWriter writer = new ExpressionWriter()
            {
                State = WritingState.Sentence
            };

            writer.Write(arg, format);

            return writer.ToString();
        }

        /// <inheritdoc/>
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
