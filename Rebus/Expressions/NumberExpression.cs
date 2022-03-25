// Ishan Pranav's REBUS: NumberExpression.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Xml;

namespace Rebus.Expressions
{
    public class NumberExpression : Expression
    {
        private readonly int _value;

        public NumberExpression(int value)
        {
            _value = value;
        }

        public override void Interpret(ICommandBuilder context)
        {
            context.Include(_value);
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(localName: "Number", XmlConvert.ToString(_value));
        }
    }
}
