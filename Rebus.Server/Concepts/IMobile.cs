// Ishan Pranav's REBUS: IMobile.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus.Server.Concepts
{
    internal interface IMobile : IWritable
    {
        HexPoint Region { get; set; }
    }
}
