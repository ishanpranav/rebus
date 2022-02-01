// Ishan Pranav's REBUS: MessageFactory.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus
{
    public abstract class MessageFactory
    {
        public async Task<RebusException> CreateExceptionAsync(string resource, params object[] arguments)
        {
            return new RebusException(await CreateMessageAsync(resource, arguments));
        }

        public abstract Task<IWritable> CreateMessageAsync(string resource, params object[] arguments);
    }
}
