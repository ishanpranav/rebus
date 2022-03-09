// Ishan Pranav's REBUS: IEngineState.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus.Server
{
    internal interface IEngineState
    {
        Task InterpretAsync(EngineContext context, string value, ExpressionWriter writer);
    }
}
