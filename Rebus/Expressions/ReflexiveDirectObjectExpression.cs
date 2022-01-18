// Ishan Pranav's REBUS: ReflexiveDirectObjectExpression.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using System.Xml;

namespace Rebus
{
    internal class ReflexiveDirectObjectExpression : Expression
    {
        public override Task InterpretAsync(ICommandBuilder context)
        {
            context.SetReflexive();

            return Task.CompletedTask;
        }

        public override void Write(ExpressionWriter writer)
        {
            writer.Write("me");
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString(localName: "Reflexive", XmlConvert.ToString(true));
        }
    }
}