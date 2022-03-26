// Ishan Pranav's REBUS: QuotationExpression.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Xml;

namespace Rebus.Expressions
{
    public class QuotationExpression : Expression
    {
        private readonly string _value;

        public QuotationExpression(string value)
        {
            _value = value;
        }

        public override void Interpret(ICommandBuilder context)
        {
            context.Add(_value);
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(localName: "Quotation", _value);
        }
    }
}
