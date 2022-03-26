// Ishan Pranav's REBUS: IExecutable.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus
{
    public interface IExecutable
    {
        Task ExecuteAsync(ExpressionWriter writer);
    }
}
