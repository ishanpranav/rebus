// Ishan Pranav's REBUS: IEngine.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus
{
    public interface IEngine
    {
        Task InterpretAsync(string userId, string value, ExpressionWriter writer);
    }
}
