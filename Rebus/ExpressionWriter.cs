// Ishan Pranav's REBUS: ExpressionWriter.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using Rebus.WritingStates;
using System.Collections.Generic;
using System.Linq;

namespace Rebus
{
    public abstract class ExpressionWriter : IWritingContext
    {
        private IWritingState _state = new InitialWritingState();

        IWritingState IWritingContext.State
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }

        protected ExpressionWriter() : this(new InitialWritingState()) { }

        protected ExpressionWriter(IWritingState state)
        {
            this._state = state;
        }

        public void Write(object? value)
        {
            if (value is IWritable writable)
            {
                writable.Write(writer: this);
            }
            else if (value is char @char)
            {
                this.Write(@char);
            }
            else if (value is string @string)
            {
                this.Write(@string);
            }
            else if (value is IEnumerable<IToken> tokens)
            {
                this.Write(tokens);
            }
            else if (value is not null)
            {
                string? valueToString = value.ToString();

                if (valueToString is not null)
                {
                    this.Write(valueToString);
                }
            }
        }

        public void Write(IEnumerable<IToken> tokens)
        {
            foreach (IGrouping<TokenTypes, IToken> grouping in tokens
                .GroupBy(x => x.Type)
                .OrderBy(x => x.Key))
            {
                IToken[] array = grouping.ToArray();

                for (int i = 0; i < array.Length - 1; i++)
                {
                    array[i].Write(this);

                    this.Write(',');
                }

                array[array.Length - 1].Write(this);

                this.Write(' ');
            }
        }

        public void Write(char value)
        {
            this._state.Write(context: this, value);
        }

        void IWritingContext.Write(char value)
        {
            this.WriteCore(value);
        }

        public void Write(string value)
        {
            foreach (char item in value)
            {
                this._state.Write(context: this, item);
            }
        }

        protected abstract void WriteCore(char value);

        public void WriteLine()
        {
            this.WriteLineCore();

            this._state = new InitialWritingState();
        }

        protected abstract void WriteLineCore();
    }
}
