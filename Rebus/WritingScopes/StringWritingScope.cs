// Ishan Pranav's REBUS: 
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;

namespace Rebus
{
    internal class StringWritingScope : IDisposable
    {
        private readonly ScopeTypes _type;

        private bool _disposed;

        protected ExpressionWriter Writer { get; }

        public StringWritingScope(ExpressionWriter writer, ScopeTypes type)
        {
            Writer = writer;
            _type = type;

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
                    if (_type.HasFlag(ScopeTypes.SingleQuotation))
                    {
                        Writer.Write('\'');
                    }

                    if (_type.HasFlag(ScopeTypes.DoubleQuotation))
                    {
                        Writer.Write('"');
                    }

                    if (_type.HasFlag(ScopeTypes.Parenthetical))
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
