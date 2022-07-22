// Ishan Pranav's REBUS: Credentials.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net;
using System.Text.Json.Serialization;
using Rebus.Client.Windows.Attributes;
using Rebus.Client.Windows.TypeConverters;

namespace Rebus.Client.Windows
{
    internal sealed class Credentials
    {
        [TypeConverter(typeof(IPAddressConverter))]
        [LocalizedDescription(nameof(IPAddress))]
        [LocalizedDisplayName(nameof(IPAddress))]
        public IPAddress IPAddress { get; set; } = IPAddress.Loopback;

        [DefaultValue(2022)]
        [LocalizedDescription(nameof(Port))]
        [LocalizedDisplayName(nameof(Port))]
        [Range(1, 65535)]
        public int Port { get; set; } = 2022;

        [DefaultValue("")]
        [LocalizedDescription(nameof(Username))]
        [LocalizedDisplayName(nameof(Username))]
        public string Username { get; set; } = string.Empty;

        [DefaultValue("")]
        [JsonIgnore]
        [LocalizedDescription(nameof(Password))]
        [LocalizedDisplayName(nameof(Password))]
        [PasswordPropertyText(true)]
        public string Password { get; set; } = string.Empty;

        [DefaultValue(null)]
        [LocalizedDescription(nameof(Culture))]
        [LocalizedDisplayName(nameof(Culture))]
        [TypeConverter(typeof(LocalizedCultureInfoConverter))]
        public CultureInfo? Culture { get; set; }

        [Browsable(false)]
        public int PlayerId { get; set; }

        public void ApplyCulture()
        {
            if (Culture is not null)
            {
                CultureInfo.CurrentCulture = Culture;
                CultureInfo.CurrentUICulture = Culture;
            }
        }
    }
}
