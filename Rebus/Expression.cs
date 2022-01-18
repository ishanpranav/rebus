// Ishan Pranav's REBUS: Expression.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Rebus
{
    public abstract class Expression : Writable, IXmlSerializable
    {
        public virtual char Punctuation
        {
            get
            {
                return '.';
            }
        }

        public abstract Task InterpretAsync(ICommandBuilder context);

        public void WriteLine(ExpressionWriter writer)
        {
            this.Write(writer);

            writer.Write(this.Punctuation);
        }

        public override string ToString()
        {
            StringExpressionWriter result = new StringExpressionWriter();

            this.WriteLine(result);

            return result.ToString();
        }

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