// Ishan Pranav's REBUS: 
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;

namespace Rebus.WritingStates
{
    public class SentenceWritingState : IWritingState
    {
        public void Write(IWritingContext context, char value)
        {
            context.Write(value);

            if (Char.IsWhiteSpace(value))
            {
                context.State = new WhiteSpaceWritingState();
            }
            else
            {
                switch (value)
                {
                    case ':':
                    case ',':
                        context.Write(' ');

                        context.State = new WhiteSpaceWritingState();
                        break;

                    case '.':
                    case '!':
                        context.Write(' ');

                        context.State = new InitialWritingState();
                        break;
                }
            }
        }
    }
}