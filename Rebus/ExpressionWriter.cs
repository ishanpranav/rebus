// Ishan Pranav's REBUS: ExpressionWriter.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using Rebus.WritingStates;

namespace Rebus
{
    public abstract class ExpressionWriter : IWritingContext
    {
        private IWritingState _state;

        protected ExpressionWriter() : this(new SentenceWritingState()) { }

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

        public void Write(object? value)
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
            else if (value is IList values)
            {
                Write(values);
            }
            else if (value is Argument argument)
            {
                Write(argument);
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
            int count = values.Count;

            for (int i = count; i > 0; i--)
            {
                Write(values[count - i]);

                switch (i)
                {
                    case 1: break;

                    case 2:
                        if (count > 2)
                        {
                            Write(',');
                        }

                        Write(" and ");
                        break;

                    default:
                        Write(',');
                        break;
                }
            }
        }

        public void Write(Argument value) { }

        void IWritingContext.Write(char value)
        {
            WriteCore(value);
        }

        public void Write(string value)
        {
            foreach (char item in value)
            {
                _state.Write(context: this, item);
            }
        }

        protected abstract void WriteCore(char value);

        public void WriteLine()
        {
            WriteLineCore();

            _state = new InitialWritingState();
        }

        protected abstract void WriteLineCore();

        public abstract IDisposable BeginScope(ScopeTypes type);
        public abstract ExpressionWriter BeginFragment();

        public abstract override string ToString();
    }
}
