// Ishan Pranav's REBUS: LocalizedDescriptionAttribute.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel;
using Rebus.Client.Windows.Properties;

namespace Rebus.Client.Windows.Attributes
{
    internal sealed class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        public LocalizedDescriptionAttribute(string resourceName) : base(Resources.ResourceManager.GetString(resourceName + "Description") ?? resourceName) { }
    }
}
