// Ishan Pranav's REBUS: InitialWritingState.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
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
