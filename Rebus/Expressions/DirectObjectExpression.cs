// Ishan Pranav's REBUS: 
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace Rebus
{
    internal class DirectObjectExpression : Expression
    {
        private readonly IEnumerable<IToken> _adjectives;
        private readonly IToken _substantive;

        public DirectObjectExpression(IEnumerable<IToken> adjectives, IToken substantive)
        {
            this._adjectives = adjectives;
            this._substantive = substantive;
        }

        public override Task InterpretAsync(ICommandBuilder context)
        {
            return context.SetDirectObjectAsync(this._adjectives, this._substantive);
        }

        public override void Write(ExpressionWriter writer)
        {
            writer.Write(this._adjectives);

            this._substantive.Write(writer);
        }

        public override void WriteXml(XmlWriter writer)
        {
            foreach (IToken adjective in this._adjectives)
            {
                writer.WriteElementString(localName: "Adjective", adjective.Value);
            }

            writer.WriteElementString(localName: "Substantive", this._substantive.Value);
        }
    }
}