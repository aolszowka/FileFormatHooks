// -----------------------------------------------------------------------
// <copyright file="StringExtensionMethodsTests.cs" company="Ace Olszowka">
// Copyright (c) 2015 Ace Olszowka.
// </copyright>
// -----------------------------------------------------------------------

namespace FileFormatHooksTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using FileFormatHooks;
    using NUnit.Framework;

    [TestFixture]
    public class StringExtensionMethodsTests
    {
        [TestCaseSource(typeof(ContainsAny_NullArguments_ThrowsArgumentNullException_Tests))]
        public void ContainsAny_NullArguments_ThrowsArgumentNullException(string sourceString, IEnumerable<string> searchStrings)
        {
            Assert.Throws<ArgumentNullException>(() => StringExtensionMethods.ContainsAny(sourceString, searchStrings));
        }

        [TestCaseSource(typeof(ContainsAny_ValidInput_ReturnsAsExpected_Tests))]
        public void ContainsAny_ValidInput_ReturnsAsExpected(string sourceString, IEnumerable<string> searchStrings, bool expected)
        {
            bool actual = StringExtensionMethods.ContainsAny(sourceString, searchStrings);

            Assert.That(actual, Is.EqualTo(expected), _Generate_ContainsAny_ErrorMessage(sourceString, searchStrings));
        }

        private string _Generate_ContainsAny_ErrorMessage(string sourceString, IEnumerable<string> searchStrings)
        {
            return string.Format("Search String: {0}\nSearch Strings: {1}", sourceString, string.Join(";", searchStrings));
        }
    }

    /// <summary>
    /// Unit tests for the ContainsAny_NullArguments_ThrowsArgumentNullException test.
    /// </summary>
    internal class ContainsAny_NullArguments_ThrowsArgumentNullException_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCaseData(null, null).SetName("BothArgumentsNull");
            yield return new TestCaseData(null, new string[] { "blah" }).SetName("SourceStringNull");
            yield return new TestCaseData("blah", null).SetName("SearchStringsNull");
        }
    }

    /// <summary>
    /// Unit tests for the ContainsAny_ValidInput_ReturnsAsExpected test.
    /// </summary>
    internal class ContainsAny_ValidInput_ReturnsAsExpected_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCaseData("The quick brown fox jumps over the lazy dog", new string[] { "fox" }, true).SetName("Test1");
            yield return new TestCaseData("The quick brown fox jumps over the lazy dog", new string[] { "apple" }, false).SetName("Test2");
            yield return new TestCaseData("The quick brown fox jumps over the lazy dog", new string[] { "FOX" }, false).SetName("Test3");
            yield return new TestCaseData("The quick brown fox jumps over the lazy dog", new string[] { "dOg" }, false).SetName("Test4");
            yield return new TestCaseData("The quick brown fox jumps over the lazy dog", new string[] { "dOg", "dog" }, true).SetName("Test5");
            yield return new TestCaseData("The quick brown fox jumps over the lazy dog", new string[] { "fox", "dog" }, true).SetName("Test6");
        }
    }
}
