// Ishan Pranav's REBUS: IEngine.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus
{
    public interface IEngine : IAsyncSupportInitialize
    {
        Task InterpretAsync(string playerTag, string value, ExpressionWriter writer);
    }
}
