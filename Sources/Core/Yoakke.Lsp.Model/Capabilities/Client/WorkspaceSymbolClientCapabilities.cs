// Copyright (c) 2021 Yoakke.
// Licensed under the Apache License, Version 2.0.
// Source repository: https://github.com/LanguageDev/Yoakke

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Yoakke.Lsp.Model.LanguageFeatures;

namespace Yoakke.Lsp.Model.Capabilities.Client
{
    /// <summary>
    /// See https://microsoft.github.io/language-server-protocol/specifications/specification-current/#workspace_symbol.
    /// </summary>
    public class WorkspaceSymbolClientCapabilities
    {
        /// <summary>
        /// See https://microsoft.github.io/language-server-protocol/specifications/specification-current/#workspace_symbol.
        /// </summary>
        public class SymbolKindCapabilities
        {
            /// <summary>
            /// The symbol kind values the client supports. When this
            /// property exists the client also guarantees that it will
            /// handle values outside its set gracefully and falls back
            /// to a default value when unknown.
            ///
            /// If this property is not present the client only supports
            /// the symbol kinds from `File` to `Array` as defined in
            /// the initial version of the protocol.
            /// </summary>
            [JsonProperty("valueSet", NullValueHandling = NullValueHandling.Ignore)]
            public IReadOnlyList<SymbolKind>? ValueSet { get; set; }
        }

        /// <summary>
        /// See https://microsoft.github.io/language-server-protocol/specifications/specification-current/#workspace_symbol.
        /// </summary>
        public class TagSupportCapabilities
        {
            /// <summary>
            /// The tags supported by the client.
            /// </summary>
            [JsonProperty("valueSet")]
            public IReadOnlyList<SymbolTag> ValueSet { get; set; } = Array.Empty<SymbolTag>();
        }

        /// <summary>
        /// Symbol request supports dynamic registration.
        /// </summary>
        [JsonProperty("dynamicRegistration", NullValueHandling = NullValueHandling.Ignore)]
        public bool? DynamicRegistration { get; set; }

        /// <summary>
        /// Specific capabilities for the `SymbolKind` in the `workspace/symbol`
        /// request.
        /// </summary>
        [JsonProperty("symbolKind", NullValueHandling = NullValueHandling.Ignore)]
        public SymbolKindCapabilities? SymbolKind { get; set; }

        /// <summary>
        /// The client supports tags on `SymbolInformation`.
        /// Clients supporting tags have to handle unknown tags gracefully.
        /// </summary>
        [Since(3, 16, 0)]
        [JsonProperty("tagSupport", NullValueHandling = NullValueHandling.Ignore)]
        public TagSupportCapabilities? TagSupport { get; set; }
    }
}