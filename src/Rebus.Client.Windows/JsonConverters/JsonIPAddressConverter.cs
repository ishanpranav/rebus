// Ishan Pranav's REBUS: JsonIPAddressConverter.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rebus.Client.Windows.JsonConverters
{
    internal sealed class JsonIPAddressConverter : JsonConverter<IPAddress>
    {
        public override IPAddress? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? ipString = reader.GetString();

            if (ipString is null)
            {
                return null;
            }
            else
            {
                return IPAddress.Parse(ipString);
            }
        }

        public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
