// Ishan Pranav's REBUS: QuotationExpression.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Xml;

namespace Rebus.Expressions
{
    /// <summary>
    /// Represents a quotation expression.
    /// </summary>
    public class QuotationExpression : Expression
    {
        private readonly string _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuotationExpression"/> class.
        /// </summary>
        /// <param name="value">The numeric value.</param>
        public QuotationExpression(string value)
        {
            _value = value;
        }

        /// <inheritdoc/>
        public override void Interpret(ICommandBuilder context)
        {
            context.Add(_value);
        }

        /// <inheritdoc/>
        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(localName: "Quotation", _value);
        }
    }
}
