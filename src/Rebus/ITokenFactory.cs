// Ishan Pranav's REBUS: ITokenFactory.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus
{
    /// <summary>
    /// Defines a method for asynchronously creating a token.
    /// </summary>
    public interface ITokenFactory
    {
        /// <summary>
        /// Asynchronously creates a token.
        /// </summary>
        /// <param name="value">The token value.</param>
        /// <returns>A task that represents the asynchronous create operation. The task result contains a new token instance.</returns>
        Task<IToken> CreateTokenAsync(string value);
    }
}
