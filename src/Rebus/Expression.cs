// Ishan Pranav's REBUS: Expression.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Rebus
{
    public abstract class Expression : IXmlSerializable
    {
        public abstract void Interpret(ICommandBuilder context);

        XmlSchema? IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public abstract void WriteXml(XmlWriter writer);
    }
}
