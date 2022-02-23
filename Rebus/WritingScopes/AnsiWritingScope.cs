// Ishan Pranav's REBUS: AnsiColorScope.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

namespace Rebus.WritingScopes
{
    internal sealed class AnsiWritingScope : StringWritingScope
    {
        private bool _disposed;

        private const string Prefix = "\x1B[";
        private const char Suffix = 'm';

        public AnsiWritingScope(ExpressionWriter writer, ScopeTypes type) : base(writer, type)
        {
            if (type.HasFlag(ScopeTypes.Keyword))
            {
                writer.Write(Prefix);
                writer.Write('1');
                writer.Write(Suffix);

                type &= ScopeTypes.Keyword;
            }

            switch (type)
            {
                case ScopeTypes.Noun:
                    writeAffixed('1');
                    break;

                case ScopeTypes.VerbPhrase:
                    writeAffixed('2');
                    break;
            }

            void writeAffixed(char code)
            {
                writer.Write(Prefix);
                writer.Write('3');
                writer.Write(code);
                writer.Write(Suffix);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Writer.Write(Prefix);
                    Writer.Write("39");
                    Writer.Write(Suffix);
                    Writer.Write(Prefix);
                    Writer.Write("22");
                    Writer.Write(Suffix);
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
