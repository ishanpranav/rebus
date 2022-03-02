// Ishan Pranav's REBUS: MarkdownExpressionWriter.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using Rebus.WritingScopes;
using Rebus.WritingStates;

namespace Rebus.ExpressionWriters
{
    public class MarkdownExpressionWriter : StringExpressionWriter
    {
        public MarkdownExpressionWriter() { }
        private protected MarkdownExpressionWriter(IWritingState state) : base(state) { }

        public override IDisposable CreateScope(ScopeType type)
        {
            return new MarkdownWritingScope(writer: this, type);
        }

        public override ExpressionWriter CreateFragment()
        {
            return new MarkdownExpressionWriter(new SentenceWritingState());
        }
    }
}
