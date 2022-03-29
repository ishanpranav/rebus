// Ishan Pranav's REBUS: IRepository.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus
{
    public interface IRepository
    {
        Task<IFeature?> GetStarAsync(HexPoint region);
        Task<object?> GetPlanetAsync(HexPoint region);

        Task<int> GetWealthAsync(int playerId);
        Task SetWealthAsync(int playerId, int value);

        Task AddNavigationAsync(int playerId, HexPoint region, Generator generator);
    }
}
