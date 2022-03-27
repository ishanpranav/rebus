// Ishan Pranav's REBUS: CollectionExpression.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Xml;

namespace Rebus.Expressions
{
    /// <summary>
    /// Represents a collection expression.
    /// </summary>
    public class CollectionExpression : Expression
    {
        private readonly IReadOnlyList<Expression> _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionExpression"/> class.
        /// </summary>
        /// <param name="items">The item expressions.</param>
        public CollectionExpression(IReadOnlyList<Expression> items)
        {
            _items = items;
        }

        /// <inheritdoc/>
        public override void Interpret(ICommandBuilder context)
        {
            foreach (Expression item in _items)
            {
                item.Interpret(context);
            }
        }

        /// <inheritdoc/>
        public override void WriteXml(XmlWriter writer)
        {
            foreach (Expression item in _items)
            {
                writer.WriteStartElement("Item");

                item.WriteXml(writer);

                writer.WriteEndElement();
            }
        }
    }
}
