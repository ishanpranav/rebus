// Ishan Pranav's REBUS: IPathfinder.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus
{
    public interface IPathfinder<T>
    {
        Stack<T> GetSteps(T source, T destination);
    }
}
