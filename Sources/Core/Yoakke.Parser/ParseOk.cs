// Copyright (c) 2021 Yoakke.
// Licensed under the Apache License, Version 2.0.
// Source repository: https://github.com/LanguageDev/Yoakke

namespace Yoakke.Parser
{
    /// <summary>
    /// Represents a successful parse.
    /// </summary>
    /// <typeparam name="T">The value of the parse.</typeparam>
    public readonly struct ParseOk<T>
    {
        /// <summary>
        /// The resulted parse value.
        /// </summary>
        public readonly T Value;

        /// <summary>
        /// The offset in the number of tokens.
        /// </summary>
        public readonly int Offset;

        /// <summary>
        /// The furthest <see cref="ParseError"/> found so far.
        /// </summary>
        public readonly ParseError? FurthestError;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseOk{T}"/> struct.
        /// </summary>
        /// <param name="value">The parsed value.</param>
        /// <param name="offset">The offset in the number of tokens.</param>
        /// <param name="furthestError">The furthest <see cref="ParseError"/> found so far.</param>
        public ParseOk(T value, int offset, ParseError? furthestError = null)
        {
            this.Value = value;
            this.Offset = offset;
            this.FurthestError = furthestError;
        }

        /// <summary>
        /// Implicit conversion to extract the resulting value.
        /// </summary>
        /// <param name="ok">The <see cref="ParseOk{T}"/> to cast.</param>
        public static implicit operator T(ParseOk<T> ok) => ok.Value;
    }
}
