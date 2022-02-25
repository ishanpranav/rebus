// Ishan Pranav's REBUS: MarkdownExpressionWriter.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
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

        public override IDisposable BeginScope(ScopeTypes type)
        {
            return new MarkdownWritingScope(writer: this, type);
        }

        public override ExpressionWriter BeginFragment()
        {
            return new MarkdownExpressionWriter(new SentenceWritingState());
        }
    }
}
