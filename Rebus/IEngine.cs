// Ishan Pranav's REBUS: IEngine.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus
{
    public interface IEngine<T>
    {
        Task InterpretAsync(T user, string value, ExpressionWriter writer);
    }
}
