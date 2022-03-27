// Ishan Pranav's REBUS: Expression.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Rebus
{
    /// <summary>
    /// Defines the core behavior of an abstract syntax tree and provides a base for derived classes.
    /// </summary>
    public abstract class Expression : IXmlSerializable
    {
        /// <summary>
        /// Called from constructors in derived classes to initialize the <see cref="Expression"/> class.	
        /// </summary>
        protected Expression() { }

        /// <summary>
        /// Interprets the expression.
        /// </summary>
        /// <param name="context">The interpretation context.</param>
        public abstract void Interpret(ICommandBuilder context);

        /// <inheritdoc/>
        /// <remarks>This member is an explicit interface member implementation. It can be used only when the <see cref="Expression"/> instance is cast to an <see cref="IXmlSerializable"/> interface.</remarks>
        XmlSchema? IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException">In all cases.</exception>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public abstract void WriteXml(XmlWriter writer);
    }
}
