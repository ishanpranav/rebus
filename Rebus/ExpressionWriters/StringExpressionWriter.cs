// Ishan Pranav's REBUS: StringExpressionWriter.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Text;
using Rebus.WritingScopes;
using Rebus.WritingStates;

namespace Rebus.ExpressionWriters
{
    public class StringExpressionWriter : ExpressionWriter
    {
        protected StringBuilder StringBuilder { get; } = new StringBuilder();

        public StringExpressionWriter() { }
        private protected StringExpressionWriter(IWritingState state) : base(state) { }

        protected override void WriteCore(char value)
        {
            StringBuilder.Append(value);
        }

        protected override void WriteLineCore()
        {
            StringBuilder.AppendLine();
        }

        public override IDisposable BeginScope(ScopeTypes type)
        {
            return new StringWritingScope(writer: this, type);
        }

        public override ExpressionWriter BeginFragment()
        {
            return new StringExpressionWriter(new SentenceWritingState());
        }

        public override string ToString()
        {
            return StringBuilder.ToString();
        }
    }
}
