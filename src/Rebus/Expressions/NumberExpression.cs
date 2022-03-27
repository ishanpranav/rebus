// Ishan Pranav's REBUS: NumberExpression.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Xml;

namespace Rebus.Expressions
{
    /// <summary>
    /// Represents a number expression.
    /// </summary>
    public class NumberExpression : Expression
    {
        private readonly int _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberExpression"/> class.
        /// </summary>
        /// <param name="value">The numeric value.</param>
        public NumberExpression(int value)
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
            writer.WriteElementString(localName: "Number", XmlConvert.ToString(_value));
        }
    }
}
