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
    public class ExpressionWriter
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public WritingState State { get; set; }

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

        public void Write(object? value)
        {
            Write(value, format: null);
        }

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

        public void Write(int value)
        {
            Write(value.ToString("n0"));
        }

        public void Write(Enum value)
        {
            Write(value
                .ToString()
                .ToLower());
        }

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

        public void Write(string value)
        {
            foreach (char @char in value)
            {
                Write(@char);
            }
        }

        public void Write(StringBuilder value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                Write(value[i]);
            }
        }

        public void WriteLine()
        {
            _stringBuilder.AppendLine();

            State = WritingState.Initial;
        }

        public void Wrap(IWrapper wrapper)
        {
            wrapper.Wrap(_stringBuilder);
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}
