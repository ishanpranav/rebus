// Ishan Pranav's REBUS: StringExpressionWriter.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Text;

namespace Rebus.ExpressionWriters
{
    public class StringExpressionWriter : ExpressionWriter
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public StringExpressionWriter() { }
        private protected StringExpressionWriter(IWritingState state) : base(state) { }

        protected override void WriteCore(char value)
        {
            _stringBuilder.Append(value);
        }

        protected override void WriteLineCore()
        {
            _stringBuilder.AppendLine();
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}
