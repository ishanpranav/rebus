// Ishan Pranav's REBUS: ExpressionWriter.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rebus
{
    /// <summary>
    /// Represents an expression writer that formats objects as well-formed English sentences.
    /// </summary>
    public class ExpressionWriter
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        /// <summary>
        /// Gets or sets the writing state.
        /// </summary>
        /// <value>The writing state. The default is <see cref="WritingState.Initial"/>.</value>
        public WritingState State { get; set; }

        /// <summary>
        /// Writes the well-formed string representation of a character literal.
        /// </summary>
        /// <param name="value">The character.</param>
        public void Write(char value)
        {
            switch (State)
            {
                case WritingState.Initial:
                    writeInitial();
                    break;

                case WritingState.Sentence:
                    _stringBuilder.Append(value);

                    if (char.IsWhiteSpace(value))
                    {
                        State = WritingState.WhiteSpace;
                    }
                    else
                    {
                        switch (value)
                        {
                            case ',':
                                _stringBuilder.Append(' ');

                                State = WritingState.WhiteSpace;
                                break;

                            case '.':
                                State = WritingState.FullStop;
                                break;

                            case ':':
                            case '!':
                            case '?':
                                _stringBuilder.Append(' ');

                                State = WritingState.Initial;
                                break;
                        }
                    }
                    break;

                case WritingState.WhiteSpace:
                    if (!char.IsWhiteSpace(value))
                    {
                        _stringBuilder.Append(value);

                        if (char.IsLetterOrDigit(value))
                        {
                            State = WritingState.Sentence;
                        }
                    }
                    break;

                case WritingState.FullStop:
                    if (!char.IsDigit(value) && !char.IsWhiteSpace(value))
                    {
                        _stringBuilder.Append(' ');
                    }

                    writeInitial();
                    break;

                    void writeInitial()
                    {
                        if (!char.IsWhiteSpace(value))
                        {
                            _stringBuilder.Append(char.ToUpper(value));

                            State = WritingState.Sentence;
                        }
                    }
            }
        }

        /// <summary>
        /// Writes the well-formed string representation of an object.
        /// </summary>
        /// <param name="value">The object.</param>
        public void Write(object? value)
        {
            Write(value, format: null);
        }

        /// <inheritdoc cref="Write(object)"/>
        /// <param name="value">The object.</param>
        /// <param name="format">The format string.</param>
        public void Write(object? value, string? format)
        {
            if (value is not null)
            {
                if (value is IWritable writable)
                {
                    writable.Write(writer: this);
                }
                else if (value is Enum @enum)
                {
                    Write(@enum);
                }
                else if (value is string @string)
                {
                    Write(@string);
                }
                else if (value is IEnumerable list)
                {
                    Write(list
                        .Cast<object>()
                        .ToArray(), format);
                }
                else if (value is char @char)
                {
                    Write(@char);
                }
                else if (value is int int32)
                {
                    Write(int32, format);
                }
                else if (value is IFormattable formattable)
                {
                    Write(formattable.ToString(format, formatProvider: null));
                }
                else
                {
                    string? valueToString = value.ToString();

                    if (valueToString is not null)
                    {
                        Write(valueToString);
                    }
                }
            }
        }

        /// <summary>
        /// Writes the well-formed string representation of a number.
        /// </summary>
        /// <param name="value">The number.</param>
        /// <param name="format">The format string.</param>
        public void Write(int value, string? format)
        {
            if (format is null)
            {
                Write(value.ToString("n0"));
            }
            else if (format.Contains(','))
            {
                string[] substrings = format.Split(',');

                if (substrings.Length > 1)
                {
                    if (value == 1)
                    {
                        Write(substrings[0]);
                    }
                    else
                    {
                        Write(substrings[1]);
                    }
                }
                else if (substrings.Length > 0)
                {
                    Write(substrings[0]);
                }
            }
            else
            {
                Write(value.ToString(format));
            }
        }

        /// <summary>
        /// Writes the well-formed string representation of an enumeration value.
        /// </summary>
        /// <param name="value">The enumeration value.</param>
        public void Write(Enum value)
        {
            Write(value
                .ToString()
                .ToLower());
        }

        /// <summary>
        /// Writes the well-formed string representation of a collection.
        /// </summary>
        /// <param name="values">The collection.</param>
        /// <param name="conjunction">The conjunction.</param>
        public void Write<T>(IReadOnlyList<T> values, string? conjunction)
        {
            int count = values.Count;

            for (int i = count; i > 0; i--)
            {
                Write(values[count - i], conjunction);

                switch (i)
                {
                    case 1: break;

                    case 2:
                        if (count > 2)
                        {
                            Write(',');
                        }

                        Write(' ');

                        if (conjunction is not null)
                        {
                            Write(conjunction);
                            Write(' ');
                        }
                        break;

                    default:
                        Write(',');
                        break;
                }
            }
        }

        /// <summary>
        /// Writes the well-formed string representation of a string literal.
        /// </summary>
        /// <param name="value">The string.</param>
        public void Write(string value)
        {
            foreach (char @char in value)
            {
                Write(@char);
            }
        }

        /// <summary>
        /// Writes the well-formed string representation of a string builder.
        /// </summary>
        /// <param name="value">The string builder.</param>
        public void Write(StringBuilder value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes a line terminator.
        /// </summary>
        public void WriteLine()
        {
            _stringBuilder.AppendLine();

            State = WritingState.Initial;
        }

        /// <summary>
        /// Wraps the text in the writer using a wrapper.
        /// </summary>
        /// <param name="wrapper">The wrapper.</param>
        public void Wrap(IWrapper wrapper)
        {
            wrapper.Wrap(_stringBuilder);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}
