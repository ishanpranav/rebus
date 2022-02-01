// Ishan Pranav's REBUS: SentenceExpression.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using System.Xml;

namespace Rebus.Expressions
{
    internal sealed class SentenceExpression : Expression
    {
        private readonly Expression _subject;
        private readonly Expression _verbPhrase;
        private readonly Expression? _directObject;

        public override char Punctuation
        {
            get
            {
                return _subject.Punctuation;
            }
        }

        public SentenceExpression(Expression subject, Expression verbPhrase, Expression? directObject)
        {
            _subject = subject;
            _verbPhrase = verbPhrase;
            _directObject = directObject;
        }

        public override async Task InterpretAsync(ICommandBuilder context)
        {
            await _verbPhrase.InterpretAsync(context);
            await _subject.InterpretAsync(context);

            if (_directObject is not null)
            {
                await _directObject.InterpretAsync(context);
            }

            await context.SaveChangesAsync();
        }

        public override void Write(ExpressionWriter writer)
        {
            _subject.Write(writer);

            writer.Write(' ');

            _verbPhrase.Write(writer);

            if (_directObject is not null)
            {
                writer.Write(' ');

                _directObject.Write(writer);
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Subject");

            _subject.WriteXml(writer);

            writer.WriteEndElement();

            writer.WriteStartElement("VerbPhrase");

            _verbPhrase.WriteXml(writer);

            writer.WriteEndElement();

            if (_directObject is not null)
            {
                writer.WriteStartElement("DirectObject");

                _directObject.WriteXml(writer);

                writer.WriteEndElement();
            }
        }
    }
}
