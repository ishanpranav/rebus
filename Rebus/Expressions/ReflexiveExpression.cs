// Ishan Pranav's REBUS: ReflexiveExpression.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using System.Xml;

namespace Rebus.Expressions
{
    internal sealed class ReflexiveExpression : Expression
    {
        private readonly Argument _argument;
        private readonly bool _firstPerson;

        public ReflexiveExpression(Argument directObject, bool firstPerson)
        {
            _argument = directObject;
            _firstPerson = firstPerson;
        }

        public override Task InterpretAsync(ICommandBuilder context)
        {
            context.Arguments.SetReflexive(_argument);

            return Task.CompletedTask;
        }

        public override void Write(ExpressionWriter writer)
        {
            writer.Write(_argument);

            if (_firstPerson)
            {
                if (_argument is Argument.Subject)
                {
                    writer.Write('I');
                }
                else
                {
                    writer.Write("myself");
                }
            }
            else
            {
                writer.Write("you");
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
            if (_firstPerson)
            {
                writer.WriteElementString(localName: "FirstPerson", value: null);
            }
            else
            {
                writer.WriteElementString(localName: "SecondPerson", value: null);
            }
        }
    }
}
