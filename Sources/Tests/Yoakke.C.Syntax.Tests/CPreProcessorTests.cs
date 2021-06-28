// Copyright (c) 2021 Yoakke.
// Licensed under the Apache License, Version 2.0.
// Source repository: https://github.com/LanguageDev/Yoakke

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yoakke.C.Syntax.Tests
{
    [TestClass]
    public class CPreProcessorTests
    {
        private static IEnumerable<object[]> TextEqualsInputs { get; } = new object[][]
        {
            TextInput(
                "a b c",
                "a", "b", "c"),

            TextInput(@"
#define FOO
FOO"),

            TextInput(@"
#define FOO BAR
FOO",
                "BAR"),

            TextInput(@"
#define FOO a b c
FOO",
                "a", "b", "c"),

            TextInput(@"
#define FOO BAR
#define BAR QUX
FOO",
                "QUX"),
        };

        private static object[] TextInput(string sourceText, params string[] tokenTexts) => new object[] { sourceText, tokenTexts };

        [DynamicData(nameof(TextEqualsInputs))]
        [DataTestMethod]
        public void TextEquals(string sourceText, string[] tokenTexts)
        {
            var lexer = new CLexer(sourceText);
            var pp = new CPreProcessor(lexer);
            foreach (var tokenText in tokenTexts)
            {
                var gotToken = pp.Next();
                Assert.AreEqual(tokenText, gotToken.LogicalText);
            }
            var end = pp.Next();
            Assert.AreEqual(CTokenType.End, end.Kind);
        }
    }
}
