// Ishan Pranav's REBUS: NounExpression.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Rebus.Expressions
{
    public class NounExpression : Expression
    {
        private readonly Argument _argument;
        private readonly IToken? _article;
        private readonly IEnumerable<IToken> _adjectives;
        private readonly IToken _substantive;

        public override char Punctuation
        {
            get
            {
                if (_argument is Argument.Subject)
                {
                    return '!';
                }
                else
                {
                    return base.Punctuation;
                }
            }
        }

        public NounExpression(Argument argument, IToken? article, IEnumerable<IToken> adjectives, IToken substantive)
        {
            _argument = argument;
            _article = article;
            _adjectives = adjectives;
            _substantive = substantive;
        }

        public override Task InterpretAsync(ICommandBuilder context)
        {
            return context.SetConceptAsync(_argument, _adjectives, _substantive);
        }

        public override void Write(ExpressionWriter writer)
        {
            writer.Write(_argument);

            if (_article is not null)
            {
                _article.Write(writer);

                writer.Write(' ');
            }

            foreach (IGrouping<TokenTypes, IToken> grouping in _adjectives
                .GroupBy(x => x.Type)
                .OrderBy(x => x.Key))
            {
                IToken[] array = grouping.ToArray();
                int lastIndex = array.Length - 1;

                for (int i = 0; i < lastIndex; i++)
                {
                    array[i].Write(writer);

                    writer.Write(',');
                }

                array[lastIndex].Write(writer);

                writer.Write(' ');
            }

            _substantive.Write(writer);

            if (_argument is Argument.Subject)
            {
                writer.Write(',');
            }
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
