// Ishan Pranav's REBUS: SystemCommand.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

namespace Rebus.Commands
{
    public abstract class SystemCommand : Command
    {
#nullable disable
        public Executor Executor { get; internal set; }
#nullable enable
    }
}
