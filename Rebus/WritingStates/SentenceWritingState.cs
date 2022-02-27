// Ishan Pranav's REBUS: SentenceWritingState.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

namespace Rebus.WritingStates
{
    internal sealed class SentenceWritingState : IWritingState
    {
        public void Write(IWritingContext context, char value)
        {
            context.Write(value);

            if (char.IsWhiteSpace(value))
            {
                context.State = new WhiteSpaceWritingState();
            }
            else
            {
                switch (value)
                {
                    case ',':
                        context.Write(' ');

                        context.State = new WhiteSpaceWritingState();
                        break;

                    case '.':
                        context.State = new FullStopWritingState();
                        break;

                    case ':':
                    case '!':
                        context.Write(' ');

                        context.State = new InitialWritingState();
                        break;
                }
            }
        }
    }
}
