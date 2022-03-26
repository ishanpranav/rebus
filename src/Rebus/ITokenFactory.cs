// Ishan Pranav's REBUS: ITokenFactory.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus
{
    public interface ITokenFactory
    {
        Task<IToken> CreateTokenAsync(string value);
    }
}