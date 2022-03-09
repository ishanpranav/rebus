// Ishan Pranav's REBUS: ArgumentSet.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Rebus.Server
{
    internal sealed class ArgumentSet
    {
        private readonly Dictionary<Argument, object> _values = new Dictionary<Argument, object>();

#nullable disable
        private Player _player;
        public Player Player
        {
            get
            {
                return _player;
            }
            set
            {
                _values.TryAdd(Argument.Subject, true);

                _player = value;
            }
        }
#nullable enable

        public int Count
        {
            get
            {
                return _values.Count;
            }
        }

        public void SetReflexive(Argument argument)
        {
            _values[argument] = _values[Argument.Subject];
        }

        public string GetQuotation(Argument argument)
        {
            return (string)_values[argument];
        }

        public bool IsQuotation(Argument argument)
        {
            return _values.TryGetValue(argument, out object? value) && value is string;
        }

        public void SetQuotation(Argument argument, string value)
        {
            _values[argument] = value;
        }

        public bool IsPlayer(Argument argument)
        {
            return _values.TryGetValue(argument, out object? value) && value is true;
        }

        public bool IsConcept(Argument subject)
        {
            return _values.TryGetValue(subject, out object? value) && value is IReadOnlyCollection<Concept>;
        }

        public bool IsPlayerOrConcept(Argument argument)
        {
            return _values.TryGetValue(argument, out object? value) && (value is true || value is IReadOnlyCollection<Concept>);
        }

        public void AddConcept(Argument argument, Concept value)
        {
            if (_values.TryGetValue(argument, out object? list) && list is ICollection<Concept> concepts)
            {
                concepts.Add(value);
            }
            else
            {
                _values[argument] = new HashSet<Concept>()
                {
                    value
                };
            }
        }

        public IReadOnlyCollection<Concept> GetConcepts(Argument argument)
        {
            return (IReadOnlyCollection<Concept>)_values[argument];
        }

        public bool TryGetConcepts(
            Argument argument,
            [NotNullWhen(true)] out IReadOnlyCollection<Concept>? results)
        {
            if (_values.TryGetValue(argument, out object? value) && value is IReadOnlyCollection<Concept> concepts)
            {
                results = concepts;

                return true;
            }
            else
            {
                results = null;

                return false;
            }
        }

        public bool TryGetNumber(Argument argument, out int result)
        {
            if (_values.TryGetValue(argument, out object? value) && value is int number)
            {
                result = number;

                return true;
            }
            else
            {
                result = 0;

                return false;
            }
        }

        public void SetNumber(Argument argument, int value)
        {
            _values[argument] = value;
        }
    }
}
