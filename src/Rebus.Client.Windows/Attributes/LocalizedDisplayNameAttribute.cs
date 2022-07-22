// Ishan Pranav's REBUS: LocalizedDisplayNameAttribute.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel;
using Rebus.Client.Windows.Properties;

namespace Rebus.Client.Windows.Attributes
{
    internal sealed class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        public LocalizedDisplayNameAttribute(string resourceName) : base(Resources.ResourceManager.GetString(resourceName + "DisplayName") ?? resourceName) { }
    }
}
