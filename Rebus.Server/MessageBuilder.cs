// Ishan Pranav's REBUS: MessageBuilder.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Localization;
using System;
using System.Linq;

namespace Rebus.Server
{
    internal class MessageBuilder<T>
    {
        private readonly Random _random;
        private readonly IStringLocalizer<T> _stringLocalizer;

        private int _index;
        private object[]? _arguments;
        private LocalizedString? _localizedString;

        public MessageBuilder(Random random, IStringLocalizer<T> stringLocalizer)
        {
            this._random = random;
            this._stringLocalizer = stringLocalizer;
        }

        public void Begin(int id, int argumentCount)
        {
            this._index = 0;
            this._arguments = new object[argumentCount];

            string prefix = $"{id}f{argumentCount}";
            LocalizedString[] localizedStrings = this._stringLocalizer
                .GetAllStrings()
                .Where(x => x.Name.StartsWith(prefix))
                .ToArray();

            this._localizedString = localizedStrings[this._random.Next(localizedStrings.Length)];
        }

        public void Append(object argument)
        {
            if (this._arguments is null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                this._arguments[this._index] = argument;

                this._index++;
            }
        }

        public IWritable Build()
        {
            if (this._arguments is null || this._localizedString is null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                return new Message(this._localizedString, this._arguments);
            }
        }
    }
}
