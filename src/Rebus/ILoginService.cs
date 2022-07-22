// Ishan Pranav's REBUS: ILoginService.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;

namespace Rebus
{
    public interface ILoginService : IDisposable
    {
        Task<int> LoginAsync(string username, string password);
        Task<int> RegisterAsync(string username, string password);
    }
}
