// Ishan Pranav's REBUS: StringWritingScope.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Rebus.WritingScopes
{
    internal class StringWritingScope : IDisposable
    {
        private bool _disposed;

        public ExpressionWriter Writer { get; }
        public ScopeType Type { get; }

        public StringWritingScope(ExpressionWriter writer, ScopeType type)
        {
            Writer = writer;
            Type = type;

            if (type is ScopeType.Parenthetical)
            {
                writer.Write('(');
            }

            if (type is ScopeType.Quotation)
            {
                writer.Write('"');
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (Type is ScopeType.Quotation)
                    {
                        Writer.Write('"');
                    }

                    if (Type is ScopeType.Parenthetical)
                    {
                        Writer.Write(')');
                    }
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
