// Ishan Pranav's REBUS: 
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using Rebus.WritingStates;

namespace Rebus.ExpressionWriters
{
    public class MarkdownExpressionWriter : StringExpressionWriter
    {
        public MarkdownExpressionWriter() { }
        private protected MarkdownExpressionWriter(IWritingState state) : base(state) { }

        public override ExpressionWriter BeginFragment()
        {
            return new MarkdownExpressionWriter(new SentenceWritingState());
        }
    }
}
