// Ishan Pranav's REBUS: ExpressionWriter.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Rebus.WritingStates;

namespace Rebus
{
    public abstract class ExpressionWriter : IWritingContext
    {
        private IWritingState _state;

        protected ExpressionWriter() : this(new InitialWritingState()) { }

        private protected ExpressionWriter(IWritingState state)
        {
            _state = state;
        }

        IWritingState IWritingContext.State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        public void Write(object? value, string? format)
        {
            if (value is not null)
            {
                if (value is IWritable writable)
                {
                    writable.Write(writer: this);
                }
                else if (value is char @char)
                {
                    Write(@char);
                }
                else if (value is string @string)
                {
                    Write(@string);
                }
                else if (value is Argument argument)
                {
                    Write(argument);
                }
                else if (value is Enum @enum)
                {
                    Write(@enum);
                }
                else if (value is IList list)
                {
                    Write(list, format);
                }
                else if (value is HexPoint hexPoint)
                {
                    Write(hexPoint);
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

        public void Write(char value)
        {
            _state.Write(context: this, value);
        }

        public void Write(int value)
        {
            Write(value.ToString("n0"));
        }

        public void Write(Argument value)
        {
            switch (value)
            {
                case Argument.IndirectObject:
                    Write(" to ");
                    break;
            }
        }

        public void Write(Enum value)
        {
            Write(value
                .ToString()
                .ToLower());
        }

        public void Write(IList values)
        {
            Write(values, conjunction: null);
        }

        public void Write(IList list, string? conjunction)
        {
            int count = list.Count;

            for (int i = count; i > 0; i--)
            {
                Write(list[count - i], conjunction);

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
        /// Writes the string representation of the specified <see cref="HexPoint"/> value to this instance.
        /// </summary>
        /// <param name="value">The <see cref="HexPoint"/> value to write.</param>
        /// <remarks>This method converts the given <see cref="HexPoint"/> value to a specialized region notation. The first section of the resulting string is the <see cref="HexPoint.Q"/> coordinate formatted as a base-26 (hexavigesimal) number where each digit represents an uppercase letter of the English alphabet (e.g., <c>'A'</c> is zero, <c>'Z'</c> is 25, <c>'AA'</c> is 26, <c>ZZ</c> is 675, etc.). The second section of the string is the Arabic-numeral representation of the <see cref="HexPoint.R"/> coordinate.</remarks>
        public void Write(HexPoint value)
        {
            int q = value.Q;
            int ones;
            Stack<char> chars = new Stack<char>();

            do
            {
                ones = q % 26;

                chars.Push((char)(ones + 'A'));

                q = (q - ones) / 26;
            }
            while (q > 0);

            while (chars.TryPop(out char result))
            {
                Write(result);
            }

            string r = value.R.ToString();

            Write(r);
            Write(" at (");
            Write(r);
            Write(',');
            Write(value.Q.ToString());
            Write(',');
            Write(value.S.ToString());
            Write(')');
        }

        void IWritingContext.Write(char value)
        {
            WriteCore(value);
        }

        public void Write(string value)
        {
            foreach (char @char in value)
            {
                _state.Write(context: this, @char);
            }
        }

        public void Write(StringBuilder value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                _state.Write(context: this, value[i]);
            }
        }

        protected abstract void WriteCore(char value);

        public void WriteLine()
        {
            WriteLineCore();

            _state = new InitialWritingState();
        }

        protected abstract void WriteLineCore();

        public abstract IDisposable CreateScope(ScopeType type);
        public abstract ExpressionWriter CreateFragment();

        public abstract override string ToString();
    }
}
