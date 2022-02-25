// Ishan Pranav's REBUS: MarkdownWritingScope.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

namespace Rebus.WritingScopes
{
    internal sealed class MarkdownWritingScope : StringWritingScope
    {
        private bool _disposed;

        public MarkdownWritingScope(ExpressionWriter writer, ScopeTypes type) : base(writer, type)
        {
            if (type.HasFlag(ScopeTypes.Noun) || type.HasFlag(ScopeTypes.VerbPhrase))
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
                    if (Type.HasFlag(ScopeTypes.Noun) || Type.HasFlag(ScopeTypes.VerbPhrase))
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
