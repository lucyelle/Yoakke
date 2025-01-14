// Copyright (c) 2021 Yoakke.
// Licensed under the Apache License, Version 2.0.
// Source repository: https://github.com/LanguageDev/Yoakke

using System;

namespace Yoakke.Parser.Attributes
{
    /// <summary>
    /// An attribute to mark a class as a parser with rule methods inside.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ParserAttribute : Attribute
    {
        /// <summary>
        /// The token kind type to use as a parser element.
        /// </summary>
        public Type? TokenType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserAttribute"/> class.
        /// </summary>
        public ParserAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserAttribute"/> class.
        /// </summary>
        /// <param name="tokenType">The token kind type to use as a parser element.</param>
        public ParserAttribute(Type tokenType)
        {
            this.TokenType = tokenType;
        }
    }
}
