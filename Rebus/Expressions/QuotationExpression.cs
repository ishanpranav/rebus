// Ishan Pranav's REBUS: QuotationExpression.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using System.Xml;

namespace Rebus.Expressions
{
    internal sealed class QuotationExpression : Expression
    {
        private readonly string _value;

        public QuotationExpression(string value)
        {
            _value = value;
        }

        public override Task InterpretAsync(ICommandBuilder context)
        {
            context.Include(_value);

            return Task.CompletedTask;
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(localName: "Quotation", _value);
        }
    }
}
