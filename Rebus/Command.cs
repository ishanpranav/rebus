// Ishan Pranav's REBUS: Command.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rebus
{
    public abstract class Command
    {
        private readonly Dictionary<Argument, object> _valuesByArgument = new Dictionary<Argument, object>();

        public Guid Guid
        {
            get
            {
                return GetType().GUID;
            }
        }

#nullable disable
        public IConcept Player { get; private set; }
#nullable enable

        protected internal abstract Task<IWritable?> ExecuteAsync();

        public void Set(Argument argument, object value)
        {
            _valuesByArgument[argument] = value;
        }

        public IConcept GetConcept(Argument argument)
        {
            return (IConcept)_valuesByArgument[argument];
        }

        public int GetNumber(Argument argument)
        {
            return (int)_valuesByArgument[argument];
        }

        public string GetQuotation(Argument argument)
        {
            return (string)_valuesByArgument[argument];
        }

        public Command Clone(IConcept player)
        {
            Command command = (Command)MemberwiseClone();

            command.Player = player;

            command._valuesByArgument.Clear();

            return command;
        }
    }
}
