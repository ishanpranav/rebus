// Ishan Pranav's REBUS: NumberExpression.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using System.Xml;

namespace Rebus.Expressions
{
    internal sealed class NumberExpression : Expression
    {
        private readonly int _value;

        public NumberExpression(int value)
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
            writer.WriteElementString(localName: "Number", XmlConvert.ToString(_value));
        }
    }
}
