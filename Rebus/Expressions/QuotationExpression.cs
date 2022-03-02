// Ishan Pranav's REBUS: QuotationExpression.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using System.Xml;

namespace Rebus.Expressions
{
    internal sealed class QuotationExpression : Expression
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

            using (writer.CreateScope(ScopeType.Quotation))
            {
                writer.Write(_value);
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(localName: "Quotation", _value);
        }
    }
}
