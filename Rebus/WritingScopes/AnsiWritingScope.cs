// Ishan Pranav's REBUS: AnsiWritingScope.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Text;
using Rebus.ExpressionWriters;

namespace Rebus.WritingScopes
{
    internal sealed class AnsiWritingScope : StringWritingScope
    {
        private const string Prefix = "\u001b[";
        private const char Suffix = 'm';

        private readonly string? _previousCode;

        private bool _disposed;

        public string Code { get; }

        public AnsiWritingScope(AnsiExpressionWriter writer, ScopeTypes type, string? previousCode) : base(writer, type)
        {
            _previousCode = previousCode;

            StringBuilder stringBuilder = new StringBuilder(Prefix);

            if (type.HasFlag(ScopeTypes.Keyword))
            {
                stringBuilder.Append('1');

                type &= ScopeTypes.Keyword;
            }
            else
            {
                stringBuilder.Append('0');
            }

            stringBuilder.Append(";3");

            switch (type)
            {
                case ScopeTypes.Noun:
                    stringBuilder.Append('1');
                    break;

                case ScopeTypes.VerbPhrase:
                    stringBuilder.Append('2');
                    break;
            }

            string code = stringBuilder
                .Append(Suffix)
                .ToString();

            writer.Write(code);

            Code = code;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_previousCode is null)
                    {
                        Writer.Write(Prefix);
                        Writer.Write('0');
                        Writer.Write(Suffix);
                    }
                    else
                    {
                        Writer.Write(_previousCode);
                    }
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
