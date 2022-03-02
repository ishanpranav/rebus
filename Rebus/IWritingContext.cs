// Ishan Pranav's REBUS: IWritingContext.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus
{
    internal interface IWritingContext
    {
        IWritingState State { get; set; }

        void Write(char value);
    }
}