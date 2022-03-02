// Ishan Pranav's REBUS: IExecutorProvider.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus
{
    public interface IExecutorProvider
    {
        Executor Executor { get; set; }
    }
}
