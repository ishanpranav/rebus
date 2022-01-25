// Ishan Pranav's REBUS: QuotationExpression.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using System.Xml;

namespace Rebus.Expressions
{
    public class QuotationExpression : Expression
    {
        private readonly Argument _argument;
        private readonly string _value;

        public QuotationExpression(Argument argument, string value)
        {
            _argument = argument;
            _value = value;
        }

        public override Task InterpretAsync(ICommandBuilder context)
        {
            context.SetQuotation(_argument, _value);

            return Task.CompletedTask;
        }

        public override void Write(ExpressionWriter writer)
        {
            writer.Write(_argument);
            writer.Write('"');
            writer.Write(_value);
            writer.Write('"');
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(localName: "Quotation", _value);
        }
    }
}
