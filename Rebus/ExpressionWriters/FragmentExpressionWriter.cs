// Ishan Pranav's REBUS: FragmentExpressionWriter.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using Rebus.WritingStates;

namespace Rebus.ExpressionWriters
{
    public class FragmentExpressionWriter : StringExpressionWriter
    {
        public FragmentExpressionWriter() : base(new SentenceWritingState()) { }
    }
}