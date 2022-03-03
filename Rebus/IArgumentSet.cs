// Ishan Pranav's REBUS: IArgumentSet.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus
{
    public interface IArgumentSet
    {
        void SetNumber(Argument argument, int value);
        void SetQuotation(Argument argument, string value);
        void SetReflexive(Argument argument);
    }
}
