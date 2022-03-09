// Ishan Pranav's REBUS: IUnexecutable.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus
{
    public interface IUnexecutable
    {
        Task UnexecuteAsync(ExpressionWriter writer);
    }
}
