// Ishan Pranav's REBUS: IExecutorProvider.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus
{
    /// <summary>
    /// Defines a method for retrieving an <see cref="Rebus.Executor"/>.
    /// </summary>
    public interface IExecutorProvider
    {
        /// <summary>
        /// Gets or sets the executor.
        /// </summary>
        /// <value>An executor.</value>
        Executor Executor { get; set; }
    }
}
