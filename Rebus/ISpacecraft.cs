// Ishan Pranav's REBUS: ISpacecraft.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus
{
    public interface ISpacecraft
    {
        int PlayerId { get; }
        HexPoint Region { get; set; }
    }
}
