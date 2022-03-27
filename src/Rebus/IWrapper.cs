// Ishan Pranav's REBUS: IWrapper.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Text;

namespace Rebus
{
    /// <summary>
    /// Defines a method for wrapping text contained in a <see cref="StringBuilder"/>.
    /// </summary>
    public interface IWrapper
    {
        /// <summary>
        /// Wraps the text contained in the string builder.
        /// </summary>
        /// <param name="value">The string builder.</param>
        void Wrap(StringBuilder value);
    }
}
