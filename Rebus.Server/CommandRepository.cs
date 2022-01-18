// Ishan Pranav's REBUS: CommandRepository.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

namespace Rebus.Server
{
    internal class CommandRepository
    {
        private readonly Dictionary<RebusCommandAttribute, Command> _commandsByAttribute = new Dictionary<RebusCommandAttribute, Command>();

        public IEnumerable<string> GetVerbs()
        {
            return this._commandsByAttribute.Keys
                .Select(x => x.Verb)
                .Distinct();
        }

        public IEnumerable<string> GetAdverbs()
        {
            HashSet<string> results = new HashSet<string>();

            foreach (RebusCommandAttribute key in this._commandsByAttribute.Keys)
            {
                if (key.Adverb is not null)
                {
                    results.Add(key.Adverb);
                }
            }

            return results;
        }

        public CommandRepository(IEnumerable<Command> commands)
        {
            foreach (Command command in commands)
            {
                foreach (RebusCommandAttribute customAttribute in command.GetAttributes())
                {
                    this._commandsByAttribute.Add(customAttribute, command);
                }
            }
        }

        public Command GetCommand(IToken verb, IToken? adverb, bool reflexive, Characteristics directObject)
        {
            return this._commandsByAttribute.Single(x => x.Key.Matches(verb, adverb, reflexive, directObject)).Value;
        }
    }
}
