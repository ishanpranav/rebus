// Ishan Pranav's REBUS: Message.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using Rebus.ExpressionWriters;

namespace Rebus.Server
{
    internal sealed class Message : Writable, ICustomFormatter, IFormatProvider
    {
        private readonly IConcept? _player;
        private readonly IConcept? _subject;
        private readonly string _format;
        private readonly object[] _arguments;

        public Message(IConcept? player, IConcept? subject, string format, object[] arguments)
        {
            _player = player;
            _subject = subject;
            _format = format;
            _arguments = arguments;
        }

        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            FragmentExpressionWriter writer = new FragmentExpressionWriter();

            writer.Write(arg);

            return writer.ToString();
        }

        public object? GetFormat(Type? formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        public override void Write(ExpressionWriter writer)
        {
            if (_subject is not null && _subject != _player)
            {
                _subject.Write(writer);

                writer.Write(':');
            }

            writer.Write(string.Format(provider: this, _format, _arguments));
        }
    }
}
