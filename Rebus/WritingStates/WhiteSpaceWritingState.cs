// Ishan Pranav's REBUS: WhiteSpaceWritingState.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus.WritingStates
{
    internal sealed class WhiteSpaceWritingState : IWritingState
    {
        public void Write(IWritingContext context, char value)
        {
            if (!char.IsWhiteSpace(value))
            {
                context.Write(value);

                context.State = new SentenceWritingState();
            }
        }
    }
}