// Ishan Pranav's REBUS: 
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

namespace Rebus
{
    public interface IWritingContext
    {
        IWritingState State { get; set; }

        void Write(char value);
    }
}