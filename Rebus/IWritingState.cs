// Ishan Pranav's REBUS: IWritingState.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus
{
    internal interface IWritingState
    {
        void Write(IWritingContext context, char value);
    }
}