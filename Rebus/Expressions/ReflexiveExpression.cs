// Ishan Pranav's REBUS: ReflexiveExpression.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using System.Xml;

namespace Rebus.Expressions
{
    public class ReflexiveExpression : Expression
    {
        private readonly Argument _argument;

        public ReflexiveExpression(Argument argument)
        {
            _argument = argument;
        }

        public override Task InterpretAsync(ICommandBuilder context)
        {
            context.SetReflexive(_argument);

            return Task.CompletedTask;
        }

        public override void Write(ExpressionWriter writer)
        {
            writer.Write(_argument);
            writer.Write("myself");
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(localName: "Reflexive", value: null);
        }
    }
}
