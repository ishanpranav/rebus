// Ishan Pranav's REBUS: FullStopWritingState.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus.WritingStates
{
    internal sealed class FullStopWritingState : InitialWritingState
    {
        public override void Write(IWritingContext context, char value)
        {
            if (!char.IsDigit(value) && !char.IsWhiteSpace(value))
            {
                context.Write(' ');
            }

            base.Write(context, value);
        }
    }
}
