// Ishan Pranav's REBUS: 
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using Rebus.WritingScopes;
using Rebus.WritingStates;

namespace Rebus.ExpressionWriters
{
    public class AnsiExpressionWriter : StringExpressionWriter
    {
        public AnsiExpressionWriter() { }
        private protected AnsiExpressionWriter(IWritingState state) : base(state) { }

        protected override void WriteCore(char value)
        {
            _stringBuilder.Append(value);
        }

        protected override void WriteLineCore()
        {
            _stringBuilder.AppendLine();
        }

        public override IDisposable BeginScope(ScopeTypes type)
        {
            return new AnsiWritingScope(writer: this, type);
        }

        public override ExpressionWriter BeginFragment()
        {
            return new AnsiExpressionWriter(new SentenceWritingState());
        }
    }
}
