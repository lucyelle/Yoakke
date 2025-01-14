// Copyright (c) 2021 Yoakke.
// Licensed under the Apache License, Version 2.0.
// Source repository: https://github.com/LanguageDev/Yoakke

using System;
using Newtonsoft.Json;

namespace Yoakke.Lsp.Model.Serialization
{
    /// <summary>
    /// A <see cref="JsonConverter"/> for <see cref="Uri"/>.
    /// </summary>
    public class UriConverter : JsonConverter<Basic.Uri>
    {
        /// <inheritdoc/>
        public override Basic.Uri ReadJson(JsonReader reader, Type objectType, Basic.Uri existingValue, bool hasExistingValue, JsonSerializer serializer) =>
            new((string?)reader.Value ?? string.Empty);

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, Basic.Uri value, JsonSerializer serializer) =>
            writer.WriteValue(value.Value);
    }
}
