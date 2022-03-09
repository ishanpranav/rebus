// Ishan Pranav's REBUS: NounExpression.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace Rebus.Expressions
{
    public class NounExpression : Expression
    {
        private readonly IToken? _article;
        private readonly IReadOnlyCollection<IToken> _adjectives;
        private readonly IToken _substantive;

        public NounExpression(IToken? article, IReadOnlyCollection<IToken> adjectives, IToken substantive)
        {
            _article = article;
            _adjectives = adjectives;
            _substantive = substantive;
        }

        public override Task InterpretAsync(ICommandBuilder context)
        {
            context.Include(_adjectives, _substantive);

            return Task.CompletedTask;
        }

        public override void WriteXml(XmlWriter writer)
        {
            if (_article is not null)
            {
                writer.WriteElementString(localName: "Article", _article.Value);
            }

            foreach (IToken adjective in _adjectives)
            {
                writer.WriteElementString(localName: "Adjective", adjective.Value);
            }

            writer.WriteElementString(localName: "Substantive", _substantive.Value);
        }
    }
}
