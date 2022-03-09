// Ishan Pranav's REBUS: SentenceExpression.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace Rebus.Expressions
{
    internal sealed class SentenceExpression : Expression
    {
        private readonly IDictionary<Argument, Expression> _nouns;
        private readonly Expression _verbPhrase;

        public SentenceExpression(IDictionary<Argument, Expression> nouns, Expression verbPhrase)
        {
            _verbPhrase = verbPhrase;
            _nouns = nouns;
        }

        public override async Task InterpretAsync(ICommandBuilder context)
        {
            await _verbPhrase.InterpretAsync(context);

            foreach (KeyValuePair<Argument, Expression> noun in _nouns)
            {
                context.Argument = noun.Key;

                await noun.Value.InterpretAsync(context);
            }

            context.MoveNext();
        }

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
