// Ishan Pranav's REBUS: IGameService.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rebus
{
    public interface IGameService : IDisposable
    {
        Task<int> GetRadiusAsync();
        IAsyncEnumerable<ZoneResult> GetZonesAsync(int playerId);
        Task<string> RenameAsync(int unitId, string name);
        Task MoveAsync(int unitId, HexPoint destination);
    }
}
