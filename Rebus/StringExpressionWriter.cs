// Ishan Pranav's REBUS: StringExpressionWriter.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Text;

namespace Rebus
{
    public class StringExpressionWriter : ExpressionWriter
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public StringExpressionWriter() { }
        public StringExpressionWriter(IWritingState state) : base(state) { }

        protected override void WriteCore(char value)
        {
            this._stringBuilder.Append(value);
        }

        protected override void WriteLineCore()
        {
            this._stringBuilder.AppendLine();
        }

        public override string ToString()
        {
            return this._stringBuilder.ToString();
        }
    }
}
