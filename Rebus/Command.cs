// Ishan Pranav's REBUS: Command.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;

namespace Rebus
{
    public abstract class Command
    {
#nullable disable
        public IConcept Player { get; private set; }
        public IConcept Subject { get; private set; }
#nullable enable

        public IConcept? DirectObject { get; private set; }

        protected internal abstract Task<IWritable> ExecuteAsync();

        public Command Clone(IConcept player, IConcept subject, IConcept? directObject)
        {
            Command command = (Command)this.MemberwiseClone();

            command.Player = player;
            command.Subject = subject;
            command.DirectObject = directObject;

            return command;
        }

        public RebusCommandAttribute[] GetAttributes()
        {
            return Array.ConvertAll(Attribute.GetCustomAttributes(this.GetType(), typeof(RebusCommandAttribute)), x => (RebusCommandAttribute)x);
        }
    }
}
