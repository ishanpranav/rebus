// Ishan Pranav's REBUS: SentenceExpression.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Xml;

namespace Rebus.Expressions
{
    /// <summary>
    /// Represents a sentence expression.
    /// </summary>
    public class SentenceExpression : Expression
    {
        private readonly IDictionary<Argument, Expression> _nouns;
        private readonly Expression _verbPhrase;

        /// <summary>
        /// Initializes a new instance of the <see cref="SentenceExpression"/> class.
        /// </summary>
        /// <param name="nouns">The noun expressions organized by argument.</param>
        /// <param name="verbPhrase">The verb phrase expression.</param>
        public SentenceExpression(IDictionary<Argument, Expression> nouns, Expression verbPhrase)
        {
            _verbPhrase = verbPhrase;
            _nouns = nouns;
        }

        /// <inheritdoc/>
        public override void Interpret(ICommandBuilder context)
        {
            _verbPhrase.Interpret(context);

            foreach (KeyValuePair<Argument, Expression> noun in _nouns)
            {
                context.Argument = noun.Key;

                noun.Value.Interpret(context);
            }

            context.MoveNext();
        }

        /// <inheritdoc/>
        public override void WriteXml(XmlWriter writer)
        {
            foreach (KeyValuePair<Argument, Expression> noun in _nouns)
            {
                writer.WriteStartElement(noun.Key.ToString());

                noun.Value.WriteXml(writer);

                writer.WriteEndElement();
            }

            writer.WriteStartElement("VerbPhrase");

            _verbPhrase.WriteXml(writer);

            writer.WriteEndElement();
        }
    }
}
