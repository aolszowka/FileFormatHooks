// -----------------------------------------------------------------------
// <copyright file="UTFUtilitiesTests.cs" company="Ace Olszowka">
// Copyright (c) 2015 Ace Olszowka.
// </copyright>
// -----------------------------------------------------------------------

namespace FileFormatHooksTests
{
    using System;
    using System.Collections;
    using System.IO;
    using FileFormatHooks;
    using NUnit.Framework;

    [TestFixture]
    public class UTFUtilitiesTests
    {
        [Test]
        public void ContainsUTF8BOM_NullArgument_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => UTFUtilities.ContainsUTF8BOM(null));
        }

        [TestCaseSource(typeof(ContainsUTF8BOM_ValidArguments_ReturnsAsExpected_Tests))]
        public void ContainsUTF8BOM_ValidArguments_ReturnsAsExpected(Stream targetStream, bool expected)
        {
            bool actual = UTFUtilities.ContainsUTF8BOM(targetStream);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }

    internal class ContainsUTF8BOM_ValidArguments_ReturnsAsExpected_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCaseData(new MemoryStream(new byte[] { 0 }), false).SetName("EmptyFile");
            yield return new TestCaseData(new MemoryStream(new byte[] { 0xEF, 0xBB }), false).SetName("PartialUTF8BOM");
            yield return new TestCaseData(new MemoryStream(new byte[] { 0x61, 0x63, 0x65 }), false).SetName("ThreeInvalidBytes");
            yield return new TestCaseData(new MemoryStream(new byte[] { 0xEF, 0xBB, 0xBF }), true).SetName("UTF8BOMOnly");
            yield return new TestCaseData(new MemoryStream(new byte[] { 0xEF, 0xBB, 0xBF, 0x61 }), true).SetName("UTF8BOMAndData");
        }
    }

}
