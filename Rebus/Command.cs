// Ishan Pranav's REBUS: Command.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Rebus
{
    public abstract class Command
    {
#nullable disable
        private object[] _arguments;

        public IConcept Player { get; private set; }
#nullable enable

        public Guid Guid
        {
            get
            {
                return GetType().GUID;
            }
        }

        protected internal abstract IAsyncEnumerable<IWritable> ExecuteAsync();

        public void Set(Argument argument, object value)
        {
            _arguments[(int)argument] = value;
        }

        public IConcept GetConcept(Argument argument)
        {
            return _arguments[(int)argument] as IConcept ?? throw new RebusException("GetConcept");
        }

        public int GetNumber(Argument argument)
        {
            return _arguments[(int)argument] as int? ?? throw new RebusException("GetNumber");
        }

        public string GetQuotation(Argument argument)
        {
            return _arguments[(int)argument] as string ?? throw new RebusException("GetQuotation");
        }

        public Command CreateCommand(IConcept player, object?[] arguments)
        {
            Command result = (Command)MemberwiseClone();

            result.Player = player;
            result._arguments = arguments;

            return result;
        }
    }
}
