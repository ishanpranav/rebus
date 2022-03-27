// Ishan Pranav's REBUS: VerbPhraseExpression.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Xml;

namespace Rebus.Expressions
{
    /// <summary>
    /// Represents a verb phrase expression.
    /// </summary>
    public class VerbPhraseExpression : Expression
    {
        private readonly IToken _verb;
        private readonly IToken? _adverb;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerbPhraseExpression"/> class.
        /// </summary>
        /// <param name="verb">The verb.</param>
        /// <param name="adverb">The adverb.</param>
        public VerbPhraseExpression(IToken verb, IToken? adverb)
        {
            _verb = verb;
            _adverb = adverb;
        }

        /// <inheritdoc/>
        public override void Interpret(ICommandBuilder context)
        {
            context.Verb = _verb;
            context.Adverb = _adverb;
        }

        /// <inheritdoc/>
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
