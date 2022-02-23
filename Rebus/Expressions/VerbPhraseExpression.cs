// Ishan Pranav's REBUS: VerbPhraseExpression.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using System.Xml;

namespace Rebus.Expressions
{
    internal sealed class VerbPhraseExpression : Expression
    {
        private readonly IToken _verb;
        private readonly IToken? _adverb;

        public VerbPhraseExpression(IToken verb, IToken? adverb)
        {
            _verb = verb;
            _adverb = adverb;
        }

        public override Task InterpretAsync(ICommandBuilder context)
        {
            return context.SetVerbPhraseAsync(_verb, _adverb);
        }

        public override void Write(ExpressionWriter writer)
        {
            using (writer.BeginScope(ScopeTypes.VerbPhrase))
            {
                using (writer.BeginScope(ScopeTypes.Keyword))
                {
                    _verb.Write(writer);
                }

                if (_adverb is not null)
                {
                    writer.Write(' ');

                    _adverb.Write(writer);
                }
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(localName: "Verb", _verb.Value);

            if (_adverb is not null)
            {
                writer.WriteElementString(localName: "Adverb", _adverb.Value);
            }
        }
    }
}
