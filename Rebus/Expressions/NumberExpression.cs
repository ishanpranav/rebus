// Ishan Pranav's REBUS: NumberExpression.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using System.Xml;

namespace Rebus.Expressions
{
    public class NumberExpression : Expression
    {
        private readonly Argument _argument;
        private readonly int _value;

        public NumberExpression(Argument argument, int value)
        {
            _argument = argument;
            _value = value;
        }

        public override Task InterpretAsync(ICommandBuilder context)
        {
            context.SetNumber(_argument, _value);

            return Task.CompletedTask;
        }

        public override void Write(ExpressionWriter writer)
        {
            writer.Write(_argument);
            writer.Write(_value);
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(localName: "Number", XmlConvert.ToString(_value));
        }
    }
}
