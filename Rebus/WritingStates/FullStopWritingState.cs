// Ishan Pranav's REBUS: 
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

namespace Rebus.WritingStates
{
    internal sealed class FullStopWritingState : InitialWritingState
    {
        public override void Write(IWritingContext context, char value)
        {
            if (!char.IsDigit(value))
            {
                context.Write(' ');
            }

            base.Write(context, value);
        }
    }
}
