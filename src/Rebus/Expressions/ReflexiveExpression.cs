// Ishan Pranav's REBUS: ReflexiveExpression.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Xml;

namespace Rebus.Expressions
{
    /// <summary>
    /// Represents a reflexive expression.
    /// </summary>
    public class ReflexiveExpression : Expression
    {
        private readonly bool _firstPerson;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflexiveExpression"/> class.
        /// </summary>
        /// <param name="firstPerson"><see langword="true"/> to indicate that the expression refers to the first person; <see langword="false"/> to indicate that the expression refers to the second person.</param>
        public ReflexiveExpression(bool firstPerson)
        {
            _firstPerson = firstPerson;
        }

        /// <inheritdoc/>
        public override void Interpret(ICommandBuilder context)
        {
            context.AddReflexive();
        }

        /// <inheritdoc/>
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
