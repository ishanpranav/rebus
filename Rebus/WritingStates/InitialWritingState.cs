// Ishan Pranav's REBUS: InitialWritingState.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;

namespace Rebus.WritingStates
{
    internal class InitialWritingState : IWritingState
    {
        public void Write(IWritingContext context, char value)
        {
            if (!Char.IsWhiteSpace(value))
            {
                context.Write(Char.ToUpper(value));

                context.State = new SentenceWritingState();
            }
        }
    }
}