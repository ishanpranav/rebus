// Ishan Pranav's REBUS: ExpressionWriter.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
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
            else if (value is int int32)
            {
                Write(int32);
            }
            else if (value is IList list)
            {
                Write(list, format);
            }
            else if (value is Argument argument)
            {
                Write(argument);
            }
            else if (value is Enum @enum)
            {
                Write(@enum);
            }
            else if (value is not null)
            {
                string? valueToString = value.ToString();

                if (valueToString is not null)
                {
                    Write(valueToString);
                }
            }
        }

        public void Write(char value)
        {
            _state.Write(context: this, value);
        }

        public void Write(int value)
        {
            Write(value.ToString("n"));
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
