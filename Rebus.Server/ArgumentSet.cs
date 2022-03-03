// Ishan Pranav's REBUS: ArgumentSet.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Rebus.Server.Concepts;

namespace Rebus.Server
{
    internal sealed class ArgumentSet : IArgumentSet
    {
        private readonly IDictionary<Argument, object> _values;

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
                _values.TryAdd(Argument.Subject, value.Signature);

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

        public ArgumentSet()
        {
            _values = new Dictionary<Argument, object>();
        }

        public void SetReflexive(Argument argument)
        {
            _values[argument] = _values[Argument.Subject];
        }

        public void SetConceptSignature(Argument argument, ConceptSignature value)
        {
            _values[argument] = value;
        }

        public void SetNumber(Argument argument, int value)
        {
            _values[argument] = value;
        }

        public void SetQuotation(Argument argument, string value)
        {
            _values[argument] = value;
        }

        public bool IsPlayer(Argument argument)
        {
            return TryGetConceptSignature(argument, out ConceptSignature? result) && result.PlayerId == Player.Id;
        }

        public bool TryGetConceptSignature(
            Argument argument,
            [NotNullWhen(true)] out ConceptSignature? result)
        {
            if (_values.TryGetValue(argument, out object? value) && value is ConceptSignature conceptSignature)
            {
                result = conceptSignature;

                return true;
            }
            else
            {
                result = null;

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

        public bool TryGetQuotation(
            Argument argument,
            [NotNullWhen(true)] out string? result)
        {
            if (_values.TryGetValue(argument, out object? value) && value is string quotation)
            {
                result = quotation;

                return true;
            }
            else
            {
                result = null;

                return false;
            }
        }
    }
}
