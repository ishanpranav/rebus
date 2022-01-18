// Ishan Pranav's REBUS: 
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;

namespace Rebus.WritingStates
{
    internal class WhiteSpaceWritingState : IWritingState
    {
        public void Write(IWritingContext context, char value)
        {
            if (!Char.IsWhiteSpace(value))
            {
                context.Write(value);

                context.State = new SentenceWritingState();
            }
        }
    }
}