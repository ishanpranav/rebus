// Ishan Pranav's REBUS: SentenceExpression.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using System.Xml;

namespace Rebus.Expressions
{
    public class SentenceExpression : Expression
    {
        private readonly Expression? _subject;
        private readonly Expression _verbPhrase;
        private readonly Expression? _directObject;

        public override char Punctuation
        {
            get
            {
                if (this._subject is null)
                {
                    return '.';
                }
                else
                {
                    return '!';
                }
            }
        }

        public SentenceExpression(Expression? subject, Expression verbPhrase, Expression? directObject)
        {
            this._subject = subject;
            this._verbPhrase = verbPhrase;
            this._directObject = directObject;
        }

        public override async Task InterpretAsync(ICommandBuilder context)
        {
            await this._verbPhrase.InterpretAsync(context);

            if (this._subject is not null)
            {
                await this._subject.InterpretAsync(context);
            }

            if (this._directObject is not null)
            {
                await this._directObject.InterpretAsync(context);
            }

            context.MoveNext();
        }

        public override void Write(ExpressionWriter writer)
        {
            if (this._subject is null)
            {
                writer.Write("I ");
            }
            else
            {
                this._subject.Write(writer);

                writer.Write(',');
            }

            this._verbPhrase.Write(writer);

            if (this._directObject is not null)
            {
                writer.Write(' ');

                this._directObject.Write(writer);
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
            if (this._subject is not null)
            {
                writer.WriteStartElement("Subject");

                this._subject.WriteXml(writer);

                writer.WriteEndElement();
            }

            writer.WriteStartElement("VerbPhrase");

            this._verbPhrase.WriteXml(writer);

            writer.WriteEndElement();

            if (this._directObject is not null)
            {
                writer.WriteStartElement("DirectObject");

                this._directObject.WriteXml(writer);

                writer.WriteEndElement();
            }
        }
    }
}