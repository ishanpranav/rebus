// Ishan Pranav's REBUS: IEngineFactory.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus
{
    public interface IEngineFactory
    {
        Task<IEngine> CreateEngineAsync();
    }
}
