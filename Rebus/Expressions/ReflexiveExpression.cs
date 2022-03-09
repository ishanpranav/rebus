// Ishan Pranav's REBUS: ReflexiveExpression.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using System.Xml;

namespace Rebus.Expressions
{
    internal sealed class ReflexiveExpression : Expression
    {
        private readonly bool _firstPerson;

        public ReflexiveExpression(bool firstPerson)
        {
            _firstPerson = firstPerson;
        }

        public override Task InterpretAsync(ICommandBuilder context)
        {
            context.IncludeReflexive();

            return Task.CompletedTask;
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
