// Ishan Pranav's REBUS: Location.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using Rebus.Tokens;

namespace Rebus.Server.Concepts
{
    internal sealed class Location : Concept
    {
        public override HexPoint Region { get; }

        public Location(HexPointToken token)
        {
            Region = token.HexPoint;
            Substantive = new Token()
            {
                Value = token.Value
            };
        }
    }
}
