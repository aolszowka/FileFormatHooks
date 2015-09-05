// -----------------------------------------------------------------------
// <copyright file="CSharpProjectFileFormatRules.cs" company="Ace Olszowka">
// Copyright (c) 2015 Ace Olszowka.
// </copyright>
// -----------------------------------------------------------------------

namespace FileFormatHooks
{
    using System;
    using System.IO;

    /// <summary>
    /// A FileRules validator for CSharp Project Files (*.csproj).
    /// </summary>
    public class CSharpProjectFileFormatRules : IFileRules
    {
        public bool IsValid(Stream s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            return UTFUtilities.ContainsUTF8BOM(s);
        }

        public bool IsValid(StreamReader sr)
        {
            if (sr == null)
            {
                throw new ArgumentNullException("sr");
            }

            return this.IsValid(sr.BaseStream);
        }
    }
}
