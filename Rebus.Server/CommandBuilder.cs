// Ishan Pranav's REBUS: CommandBuilder.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.Exceptions;

namespace Rebus.Server
{
    internal sealed class CommandBuilder : ICommandBuilder
    {
        private readonly RebusDbContext _context;
        private readonly List<Rebus.Command> _commands = new List<Rebus.Command>();
        private readonly Dictionary<Guid, Rebus.Command> _commandsByGuid;
        private readonly int _argumentCount = Enum.GetValues<Argument>().Length;

        private IToken? _verb;
        private IToken? _adverb;
        private int? _player;
        private IConcept? _playerConcept;
        private object?[] _arguments;

        public CommandBuilder(RebusDbContext context, IEnumerable<Rebus.Command> commands)
        {
            _context = context;
            _commandsByGuid = commands.ToDictionary(x => x.Guid);
            _arguments = new object?[_argumentCount];
        }

        public void SetPlayer(int player)
        {
            _player = player;

            _commands.Clear();
        }

        public async Task SetVerbPhraseAsync(IToken verb, IToken? adverb)
        {
            _verb = verb;
            _adverb = adverb;

            if (_player is null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                _playerConcept = (await _context.Players
                    .IncludeConcept()
                    .SingleAsync(x => x.Id == _player)).Concept;
            }
        }

        public void SetReflexive(Argument argument)
        {
            if (argument is Argument.Subject)
            {
                _arguments[(int)Argument.Subject] = _playerConcept;
            }
            else
            {
                _arguments[(int)argument] = _arguments[(int)Argument.Subject];
            }
        }

        public async Task SetConceptAsync(Argument argument, IEnumerable<IToken> adjectives, IToken substantive)
        {
            ILookup<int, ConceptSignature> signaturesByCount = _context.ConceptSignatures
                .Include(signature => signature.Substantive)
                .Where(signature => signature.Substantive.Value == substantive.Value)
                .Include(signature => signature.Article)
                .Include(signature => signature.Adjectives)
                .AsSplitQuery()
                .ToLookup(signature => adjectives.Count(adjective => signature.Adjectives.Any(x => adjective.Value == x.Value)));

            ConceptSignature? signature;

            try
            {
                signature = signaturesByCount[adjectives.Count()].SingleOrDefault();
            }
            catch
            {
                throw new RebusException(resource: 5);
            }

            if (signature is null)
            {
                throw new RebusException(resource: 8);
            }
            else
            {
                _arguments[(int)argument] = await _context
                    .IncludeConcepts()
                    .SingleAsync(x => x.Id == signature.ConceptId);
            }
        }

        public void SetQuotation(Argument argument, string quotation)
        {
            _arguments[(int)argument] = quotation;
        }

        public void SetNumber(Argument argument, int number)
        {
            _arguments[(int)argument] = number;
        }

        public async Task SaveChangesAsync()
        {
            if (_verb is null || _playerConcept is null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                CommandSignature? result;

                try
                {
                    result = await _context.CommandSignatures
                        .Where(signature => signature.Verb.Value == _verb.Value && signature.Adverb == _adverb)
                        .Include(signature => signature.Command)
                        .Include(signature => signature.Arguments)
                        .AsAsyncEnumerable()
                        .SingleOrDefaultAsync(signature =>
                        {
                            ArgumentSignature?[] argumentSignatures = new ArgumentSignature?[_argumentCount];

                            foreach (ArgumentSignature argumentSignature in signature.Arguments)
                            {
                                argumentSignatures[(int)argumentSignature.Argument] = argumentSignature;
                            }

                            for (int i = 0; i < _argumentCount; i++)
                            {
                                object? argument = _arguments[i];

                                if (argumentSignatures[i] is ArgumentSignature argumentSignature)
                                {
                                    if (argument is null)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        switch (argumentSignature.Type)
                                        {
                                            case ArgumentType.Concept:
                                                if (argument is not IConcept concept)
                                                {
                                                    return false;
                                                }
                                                break;

                                            case ArgumentType.Number:
                                                if (argument is not int)
                                                {
                                                    return false;
                                                }
                                                break;

                                            case ArgumentType.Quotation:
                                                if (argument is not string)
                                                {
                                                    return false;
                                                }
                                                break;

                                            case ArgumentType.ReflexiveOnly:
                                                if (argument != _playerConcept)
                                                {
                                                    return false;
                                                }
                                                break;
                                        }
                                    }
                                }
                                else if (argument is not null)
                                {
                                    return false;
                                }
                            };

                            return true;
                        });
                }
                catch
                {
                    throw new RebusException(resource: 9);
                }

                _commands.Add(_commandsByGuid[(result ?? throw new RebusException(resource: 10)).Command.Guid].CreateCommand(_playerConcept, _player.GetValueOrDefault(), _arguments));
            }

            _verb = null;
            _adverb = null;
            _playerConcept = null;
            _arguments = new object?[_argumentCount];
        }

        public IEnumerable<Rebus.Command> Build()
        {
            return _commands;
        }
    }
}
