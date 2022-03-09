// Ishan Pranav's REBUS: CollectionExpression.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace Rebus.Expressions
{
    public class CollectionExpression : Expression
    {
        private readonly IReadOnlyList<Expression> _items;

        public CollectionExpression(IReadOnlyList<Expression> items)
        {
            _items = items;
        }

        public override async Task InterpretAsync(ICommandBuilder context)
        {
            foreach (Expression item in _items)
            {
                await item.InterpretAsync(context);
            }
        }

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
