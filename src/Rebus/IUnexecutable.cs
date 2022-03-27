// Ishan Pranav's REBUS: IUnexecutable.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus
{
    /// <summary>
    /// Defines a method for asynchronously un-executing an operation. 
    /// </summary>
    public interface IUnexecutable
    {
        /// <summary>
        /// Asynchronously un-executes an operation and writes the output to an expression writer.
        /// </summary>
        /// <param name="writer">The expression writer.</param>
        /// <returns>A task that represents the asynchronous un-execute operation.</returns>
        Task UnexecuteAsync(ExpressionWriter writer);
    }
}
