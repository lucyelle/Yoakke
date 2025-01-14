// Copyright (c) 2021 Yoakke.
// Licensed under the Apache License, Version 2.0.
// Source repository: https://github.com/LanguageDev/Yoakke

using System;
using Microsoft.CodeAnalysis;
using Yoakke.Utilities.Compatibility;

namespace Yoakke.Parser.Generator.Ast
{
    internal partial class BnfAst
    {
        /// <summary>
        /// Represents a repeating, folding parse rule.
        /// This is used for left-recursion elimination.
        /// </summary>
        public class FoldLeft : BnfAst
        {
            /// <summary>
            /// The sub-element to apply.
            /// </summary>
            public BnfAst First { get; }

            /// <summary>
            /// The element to apply repeatedly after.
            /// </summary>
            public BnfAst Second { get; }

            /// <summary>
            /// The transformation method that does the folding.
            /// </summary>
            public IMethodSymbol Method { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="FoldLeft"/> class.
            /// </summary>
            /// <param name="first">The first element of the fold.</param>
            /// <param name="second">The second element of the fold, that will be repeated.</param>
            /// <param name="method">The folding transformation.</param>
            public FoldLeft(BnfAst first, BnfAst second, IMethodSymbol method)
            {
                this.First = first;
                this.Second = second;
                this.Method = method;
            }

            /// <inheritdoc/>
            public override bool Equals(BnfAst other) => other is FoldLeft fl
                && this.First.Equals(fl.First)
                && this.Second.Equals(fl.Second)
                && SymbolEqualityComparer.Default.Equals(this.Method, fl.Method);

            /// <inheritdoc/>
            public override int GetHashCode() => HashCode.Combine(this.First, this.Second, this.Method);

            /// <inheritdoc/>
            public override BnfAst Desugar() => new FoldLeft(this.First.Desugar(), this.Second.Desugar(), this.Method);

            /// <inheritdoc/>
            public override string GetParsedType(RuleSet ruleSet, TokenKindSet tokens)
            {
                var firstType = this.First.GetParsedType(ruleSet, tokens);
                var mappedType = this.Method.ReturnType.ToDisplayString();
                if (firstType != mappedType) throw new InvalidOperationException("Incompatible folded types");
                return firstType;
            }
        }
    }
}
