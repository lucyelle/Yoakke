﻿// Copyright (c) 2021 Yoakke.
// Licensed under the Apache License, Version 2.0.
// Source repository: https://github.com/LanguageDev/Yoakke

using Newtonsoft.Json;
using Yoakke.Lsp.Model.Basic;

namespace Yoakke.Lsp.Model.TextSynchronization
{
    public class DidCloseTextDocumentParams
    {
        /// <summary>
        /// The document that was closed.
        /// </summary>
        [JsonProperty("textDocument")]
        public TextDocumentIdentifier TextDocument { get; set; }
    }
}