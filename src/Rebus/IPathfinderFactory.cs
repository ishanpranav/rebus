// Ishan Pranav's REBUS: IPathfinderFactory.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus
{
    public interface IPathfinderFactory<T>
    {
        IPathfinder<T> CreatePathfinder(int playerId);
    }
}
