// Ishan Pranav's REBUS: IEngine.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Rebus.Server
{
    internal interface IEngine
    {
        Repository Repository { get; }
        Controller Controller { get; }
        IStringLocalizer Localizer { get; }

        Task InterpretAsync(int playerId, Executor executor, string value, ExpressionWriter writer);
    }
}
