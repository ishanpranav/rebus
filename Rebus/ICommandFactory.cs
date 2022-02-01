// Ishan Pranav's REBUS: ICommandFactory.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;

namespace Rebus
{
    internal interface ICommandFactory
    {
        public Guid Guid { get; }

        public Command CreateCommand(IConcept player, object?[] arguments);
    }
}
