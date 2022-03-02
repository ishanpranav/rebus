// Ishan Pranav's REBUS: InitialWritingState.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus.WritingStates
{
    internal class InitialWritingState : IWritingState
    {
        public virtual void Write(IWritingContext context, char value)
        {
            if (!char.IsWhiteSpace(value))
            {
                context.Write(char.ToUpper(value));

                context.State = new SentenceWritingState();
            }
        }
    }
}
