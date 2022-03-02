// Ishan Pranav's REBUS: Command.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Rebus.Server.Concepts;

namespace Rebus.Server
{
    internal abstract class Command : IExecutable
    {
#nullable disable
        public Player Player { get; }
#nullable enable

        private readonly Dictionary<Argument, object> _arguments = new Dictionary<Argument, object>();

        public Guid Guid
        {
            get
            {
                return GetType().GUID;
            }
        }

        public abstract IAsyncEnumerable<IWritable> ExecuteAsync();

        protected Command() { }

        protected Command(Player player)
        {
            Player = player;
        }

        public void Set(Argument argument, object value)
        {
            _arguments[argument] = value;
        }

        public ConceptSignature GetConceptSignature(Argument argument)
        {
            return (ConceptSignature)_arguments[argument];
        }

        public int GetNumber(Argument argument)
        {
            return (int)_arguments[argument];
        }

        public string GetQuotation(Argument argument)
        {
            return _arguments[argument].ToString() ?? string.Empty;
        }

        public bool IsReflexive(Argument argument)
        {
            return _arguments[argument] is true;
        }

        public Command CreateCommand(Player player, IDictionary<Argument, object> arguments)
        {
            Command command = CreateCommand(player);

            foreach (KeyValuePair<Argument, object> pair in arguments)
            {
                command._arguments.Add(pair.Key, pair.Value);
            }

            return command;
        }

        protected abstract Command CreateCommand(Player player);
    }
}
