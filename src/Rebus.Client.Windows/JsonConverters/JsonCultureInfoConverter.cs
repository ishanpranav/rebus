// Ishan Pranav's REBUS: JsonCultureInfoConverter.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rebus.Client.Windows.JsonConverters
{
    internal sealed class JsonCultureInfoConverter : JsonConverter<CultureInfo>
    {
        public override CultureInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? text = reader.GetString();

            if (text is null)
            {
                return null;
            }
            else
            {
                return new CultureInfo(text);
            }
        }

        public override void Write(Utf8JsonWriter writer, CultureInfo value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Name);
        }
    }
}
