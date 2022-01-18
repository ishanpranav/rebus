// Ishan Pranav's REBUS: ParagraphExpression.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Rebus.Expressions
{
    public class ParagraphExpression : Expression
    {
        private readonly IEnumerable<Expression> _sentences;

        public override char Punctuation
        {
            get
            {
                return ' ';
            }
        }

        public ParagraphExpression(IEnumerable<Expression> sentences)
        {
            this._sentences = sentences;
        }

        public override async Task InterpretAsync(ICommandBuilder context)
        {
            foreach (Expression clause in this._sentences)
            {
                await clause.InterpretAsync(context);
            }
        }

        public override void Write(ExpressionWriter writer)
        {
            foreach (Expression sentence in this._sentences)
            {
                sentence.WriteLine(writer);
            }

            writer.WriteLine();
        }

        public override void WriteXml(XmlWriter writer)
        {
            foreach (Expression sentence in this._sentences)
            {
                writer.WriteStartElement("Sentence");

                sentence.WriteXml(writer);

                writer.WriteEndElement();
            }
        }
    }
}