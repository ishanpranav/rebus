// Ishan Pranav's REBUS: StringWritingScope.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;

namespace Rebus
{
    internal class StringWritingScope : IDisposable
    {
        private bool _disposed;

        protected ExpressionWriter Writer { get; }
        protected ScopeTypes Type { get; }

        public StringWritingScope(ExpressionWriter writer, ScopeTypes type)
        {
            Writer = writer;
            Type = type;

            if (type.HasFlag(ScopeTypes.Parenthetical))
            {
                writer.Write('(');
            }

            if (type.HasFlag(ScopeTypes.DoubleQuotation))
            {
                writer.Write('"');
            }

            if (type.HasFlag(ScopeTypes.SingleQuotation))
            {
                writer.Write('\'');
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (Type.HasFlag(ScopeTypes.SingleQuotation))
                    {
                        Writer.Write('\'');
                    }

                    if (Type.HasFlag(ScopeTypes.DoubleQuotation))
                    {
                        Writer.Write('"');
                    }

                    if (Type.HasFlag(ScopeTypes.Parenthetical))
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
