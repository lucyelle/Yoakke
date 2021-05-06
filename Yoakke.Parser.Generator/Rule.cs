﻿using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using Yoakke.Parser.Generator.Ast;

namespace Yoakke.Parser.Generator
{
    internal class Rule
    {
        /// <summary>
        /// The name of the rule. This is on the left-hand side of the rule.
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// The AST of the grammar to match.
        /// </summary>
        public readonly BnfAst Ast;
        /// <summary>
        /// The method to call upon a successful match.
        /// </summary>
        public readonly IMethodSymbol Method;

        public Rule(string name, BnfAst ast, IMethodSymbol method)
        {
            Name = name;
            Ast = ast;
            Method = method;
        }
    }
}