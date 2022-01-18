// Ishan Pranav's REBUS: VerbPhraseExpression.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using System.Xml;

namespace Rebus.Expressions
{
    public class VerbPhraseExpression : Expression
    {
        private readonly IToken _verb;
        private readonly IToken? _adverb;

        public VerbPhraseExpression(IToken verb, IToken? adverb)
        {
            this._verb = verb;
            this._adverb = adverb;
        }

        public override Task InterpretAsync(ICommandBuilder context)
        {
            return context.SetVerbPhraseAsync(this._verb, this._adverb);
        }

        public override void Write(ExpressionWriter writer)
        {
            this._verb.Write(writer);

            if (this._adverb is not null)
            {
                writer.Write(' ');

                this._adverb.Write(writer);
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(localName: "Verb", this._verb.Value);

            if (this._adverb is not null)
            {
                writer.WriteElementString(localName: "Adverb", this._adverb.Value);
            }
        }
    }
}