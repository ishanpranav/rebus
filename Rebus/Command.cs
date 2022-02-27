// Ishan Pranav's REBUS: Command.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Rebus
{
    public abstract class Command : ICloneable
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
            return (IConcept)_arguments[(int)argument];
        }

        public int GetNumber(Argument argument)
        {
            return (int)_arguments[(int)argument];
        }

        public string GetQuotation(Argument argument)
        {
            return _arguments[(int)argument].ToString() ?? string.Empty;
        }

        public Command CreateCommand(IConcept player, object?[] arguments)
        {
            Command result = (Command)Clone();

            result.Player = player;
            result._arguments = arguments;

            return result;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
