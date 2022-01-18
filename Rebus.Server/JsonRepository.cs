// Ishan Pranav's REBUS: JsonRepository.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rebus.Server
{
    internal class JsonRepository
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly PathProvider _pathProvider;

        public JsonRepository(JsonSerializerOptions jsonSerializerOptions, PathProvider pathProvider)
        {
            this._jsonSerializerOptions = jsonSerializerOptions;
            this._pathProvider = pathProvider;
        }

        public async Task<IReadOnlyCollection<T>> GetAsync<T>()
        {
            await using (FileStream fileStream = File.OpenRead(this._pathProvider.GetPath(typeof(IReadOnlyCollection<T>), "json")))
            {
                return (await JsonSerializer.DeserializeAsync<IReadOnlyCollection<T>>(fileStream, this._jsonSerializerOptions)) ?? Array.Empty<T>();
            }
        }
    }
}
