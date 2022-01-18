// Ishan Pranav's REBUS: Message.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using Rebus.WritingStates;
using System;
using System.Collections;

namespace Rebus.Server
{
    internal class Message : Writable, ICustomFormatter, IFormatProvider
    {
        private readonly string _format;
        private readonly object[] _arguments;

        public Message(string format, object[] arguments)
        {
            this._format = format;
            this._arguments = arguments;
        }

        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            StringExpressionWriter writer = new StringExpressionWriter(new SentenceWritingState());

            if (arg is IList argList)
            {
                int count = argList.Count;

                if (count > 0)
                {
                    writer.Write(argList[0]);

                    if (count > 1)
                    {
                        writer.Write(' ');

                        for (int i = 1; i < count - 1; i++)
                        {
                            writer.Write(argList[i]);
                            writer.Write(',');
                        }

                        writer.Write("and ");
                        writer.Write(argList[count - 1]);
                    }
                }
            }
            else if (arg is IEnumerable argEnumerable)
            {
                writer.Write(String.Join(',', argEnumerable));
            }
            else
            {
                writer.Write(arg);
            }

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
            writer.Write(String.Format(provider: this, this._format, this._arguments));
        }
    }
}