// Ishan Pranav's REBUS: CommandBuilder.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rebus.Exceptions;

namespace Rebus.Server
{
    internal sealed class CommandBuilder : ICommandBuilder
    {
        private readonly RebusDbContext _context;
        private readonly List<Command> _commands = new List<Command>();
        private readonly Dictionary<Guid, Command> _commandsByGuid;

        private ArgumentSet _arguments = new ArgumentSet();

        public IToken? Verb { get; set; }
        public IToken? Adverb { get; set; }
        public Argument Argument { get; set; }

        private Player? _player;

        public CommandBuilder(RebusDbContext context, IEnumerable<Command> commands)
        {
            _context = context;
            _commandsByGuid = commands.ToDictionary(x => x.GetType().GUID);
        }

        public async Task SetPlayerAsync(int id)
        {
            _commands.Clear();
            _arguments = new ArgumentSet();

            _player = await _context.Players.FindAsync(id);

            _arguments.Player = _player;
        }


        public void Include(IReadOnlyCollection<IToken> adjectives, IToken substantive)
        {
            if (_player is null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                Concept? result;

                try
                {
                    IQueryable<Concept> concepts = _context.Concepts
                        .AsWritable()
                        .Where(concept => concept.Substantive.Value == substantive.Value);

                    if (Argument is Argument.Subject)
                    {
                        concepts = concepts.Where(concept => concept.PlayerId == _player.Id);
                    }

                    result = concepts
                        .AsEnumerable()
                        .SingleOrDefault(concept => adjectives.All(adjective => concept.Adjectives.Any(x => adjective.Value == x.TokenValue)));
                }
                catch
                {
                    throw new RebusException(resource: 5);
                }

                if (result is null)
                {
                    throw new RebusException(resource: 8);
                }
                else
                {
                    _arguments.AddConcept(Argument, result);
                }
            }
        }

        public void Include(int number)
        {
            _arguments.SetNumber(Argument, number);
        }

        public void Include(string quotation)
        {
            _arguments.SetQuotation(Argument, quotation);
        }

        public void IncludeReflexive()
        {
            _arguments.SetReflexive(Argument);
        }

        public void MoveNext()
        {
            if (Verb is null || _player is null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                Command? prototype;

                try
                {
                    string? adverb = Adverb?.Value;

                    prototype = _context.CommandSignatures
                        .Where(signature => signature.VerbValue == Verb.Value && signature.AdverbValue == adverb)
                        .Select(x => x.Guid)
                        .AsEnumerable()
                        .Select(x => _commandsByGuid[x])
                        .SingleOrDefault(x => x.Matches(_arguments));
                }
                catch
                {
                    throw new RebusException(resource: 9);
                }

                _commands.Add((prototype ?? throw new RebusException(resource: 10)).CreateCommand(_arguments));
            }

            Verb = null;
            Adverb = null;
            _player = null;
        }

        public IEnumerable<Command> Build()
        {
            return _commands;
        }
    }
}
