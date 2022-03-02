// Ishan Pranav's REBUS: Message.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text;

namespace Rebus.Server
{
    internal sealed class Message : Writable
    {
        private readonly IWritable? _player;
        private readonly IWritable? _subject;
        private readonly string _format;
        private readonly object[] _arguments;

        public Message(IWritable? player, IWritable? subject, string format, object[] arguments)
        {
            _player = player;
            _subject = subject;
            _format = format;
            _arguments = arguments;
        }

        public override void Write(ExpressionWriter writer)
        {
            if (_subject is not null && _subject != _player)
            {
                _subject.Write(writer);

                writer.Write(':');
            }

            writer.Write(new StringBuilder()
                .AppendFormat(new MessageFormatProvider(writer), _format, _arguments)
                .Replace("\r", "")
                .Replace("\n", Environment.NewLine));
        }
    }
}
