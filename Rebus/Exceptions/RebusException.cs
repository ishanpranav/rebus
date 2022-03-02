// Ishan Pranav's REBUS: RebusException.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Rebus.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the engine discovers a user error.
    /// </summary>
    [Serializable]
    public class RebusException : Exception
    {
        public int Resource
        {
            get
            {
                return (int)(Data[nameof(Resource)] ?? 0);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RebusException"/> class.
        /// </summary>
        /// <inheritdoc cref="Exception(SerializationInfo, StreamingContext)"/>
        protected RebusException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public RebusException(int resource, params object?[] arguments)
        {
            Data[nameof(Resource)] = resource;

            for (int i = 0; i < arguments.Length; i++)
            {
                Data[i] = arguments[i];
            }
        }

        public object?[] GetArguments()
        {
            ArrayList objects = new ArrayList();
            int i = 0;

            while (Data.Contains(i))
            {
                objects.Add(Data[i]);

                i++;
            }

            return objects.ToArray();
        }
    }
}
