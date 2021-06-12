﻿// Copyright (c) 2021 Yoakke.
// Licensed under the Apache License, Version 2.0.
// Source repository: https://github.com/LanguageDev/Yoakke

using Yoakke.Parser.Generator.Ast;

namespace Yoakke.Parser.Generator
{
    /// <summary>
    /// Represents a single BNF rule-definition.
    /// </summary>
    internal class Rule
    {
        /// <summary>
        /// The name of the <see cref="Rule"/>. This is on the left-hand side of the rule-definition, before the colon.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// The AST of the grammar to match.
        /// </summary>
        public BnfAst Ast { get; set; }

        /// <summary>
        /// True, if this <see cref="Rule"/> should be part of the public API
        /// </summary>
        public readonly bool PublicApi;

        /// <summary>
        /// The visual name of this <see cref="Rule"/>.
        /// </summary>
        public string VisualName { get; set; }

        public Rule(string name, BnfAst ast, bool publicApi = true)
        {
            this.Name = name;
            this.Ast = ast;
            this.PublicApi = publicApi;
            this.VisualName = name;
        }
    }
}