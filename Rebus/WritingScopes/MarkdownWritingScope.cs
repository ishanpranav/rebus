// Ishan Pranav's REBUS: MarkdownWritingScope.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus.WritingScopes
{
    internal sealed class MarkdownWritingScope : StringWritingScope
    {
        private bool _disposed;

        public MarkdownWritingScope(ExpressionWriter writer, ScopeType type) : base(writer, type)
        {
            if (type is ScopeType.Bold)
            {
                writer.Write("**");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (Type is ScopeType.Bold)
                    {
                        Writer.Write("**");
                    }
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
