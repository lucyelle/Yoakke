// Copyright (c) 2021 Yoakke.
// Licensed under the Apache License, Version 2.0.
// Source repository: https://github.com/LanguageDev/Yoakke

using System;
using Yoakke.Utilities.FiniteAutomata;

namespace Yoakke.Utilities.RegEx
{
    public partial class RegExAst
    {
        /// <summary>
        /// Represents that a construct should be repeated 1 or more times.
        /// </summary>
        public class Rep1 : RegExAst
        {
            /// <summary>
            /// The subconstruct to repeat.
            /// </summary>
            public RegExAst Subexpr { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Rep1"/> class.
            /// </summary>
            /// <param name="subexpr">The subexpression to match 1 or more times.</param>
            public Rep1(RegExAst subexpr)
            {
                this.Subexpr = subexpr;
            }

            /// <inheritdoc/>
            public override bool Equals(RegExAst other) => other is Rep1 r && this.Subexpr.Equals(r.Subexpr);

            /// <inheritdoc/>
            public override int GetHashCode() => this.Subexpr.GetHashCode();

            /// <inheritdoc/>
            public override RegExAst Desugar()
            {
                var sub = this.Subexpr.Desugar();
                return new Seq(sub, new Rep0(sub));
            }

            /// <inheritdoc/>
            public override (State Start, State End) ThompsonConstruct(DenseNfa<char> denseNfa) =>
                throw new NotSupportedException();
        }
    }
}
