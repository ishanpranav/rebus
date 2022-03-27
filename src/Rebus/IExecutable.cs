// Ishan Pranav's REBUS: IExecutable.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus
{
    /// <summary>
    /// Defines a method for asynchronously executing an operation.
    /// </summary>
    public interface IExecutable
    {
        /// <summary>
        /// Asynchronously executes an operation and writes the output to an expression writer.
        /// </summary>
        /// <param name="writer">The expression writer.</param>
        /// <returns>A task that represents the asynchronous execute operation.</returns>
        Task ExecuteAsync(ExpressionWriter writer);
    }
}
