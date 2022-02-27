// Ishan Pranav's REBUS: IPlayerRepository.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus
{
    public interface IPlayerRepository
    {
        Task<int> GetPlayerAsync(string userId);
    }
}
