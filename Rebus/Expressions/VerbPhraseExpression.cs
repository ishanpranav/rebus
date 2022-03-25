// Ishan Pranav's REBUS: VerbPhraseExpression.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Xml;

namespace Rebus.Expressions
{
    public class VerbPhraseExpression : Expression
    {
        private readonly IToken _verb;
        private readonly IToken? _adverb;

        public VerbPhraseExpression(IToken verb, IToken? adverb)
        {
            _verb = verb;
            _adverb = adverb;
        }

        public override void Interpret(ICommandBuilder context)
        {
            context.Verb = _verb;
            context.Adverb = _adverb;
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
