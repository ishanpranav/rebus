// Ishan Pranav's REBUS: Message.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text;

namespace Rebus.Server
{
    internal sealed class Message : Writable
    {
        private readonly string _format;
        private readonly object[] _arguments;

        public Message(string format, object[] arguments)
        {
            _format = format;
            _arguments = arguments;
        }

        public override void Write(ExpressionWriter writer)
        {
            writer.Write(new StringBuilder()
                .AppendFormat(new MessageFormatProvider(writer), _format, _arguments)
                .Replace("\r", "")
                .Replace("\n", Environment.NewLine));
            writer.WriteLine();
        }
    }
}
