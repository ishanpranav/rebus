// Ishan Pranav's REBUS: AnsiExpressionWriter.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using Rebus.WritingScopes;
using Rebus.WritingStates;

namespace Rebus.ExpressionWriters
{
    public class AnsiExpressionWriter : StringExpressionWriter
    {
        private string? _previousCode;

        public AnsiExpressionWriter() { }
        private protected AnsiExpressionWriter(IWritingState state) : base(state) { }

        public override IDisposable BeginScope(ScopeTypes type)
        {
            AnsiWritingScope result = new AnsiWritingScope(writer: this, type, _previousCode);

            _previousCode = result.Code;

            return result;
        }

        public override ExpressionWriter BeginFragment()
        {
            return new AnsiExpressionWriter(new SentenceWritingState());
        }
    }
}
