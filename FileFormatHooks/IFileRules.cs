// -----------------------------------------------------------------------
// <copyright file="IFileRules.cs" company="Ace Olszowka">
// Copyright (c) 2015 Ace Olszowka.
// </copyright>
// -----------------------------------------------------------------------

namespace FileFormatHooks
{
    using System;
    using System.IO;

    /// <summary>
    /// An Interface for describing FileRules.
    /// </summary>
    public interface IFileRules
    {
        /// <summary>
        ///     Determine if the given <see cref="T:System.IO.StreamReader"/>
        /// represents a file which conforms to all rules.
        /// </summary>
        /// <param name="sr">A <see cref="T:System.IO.StreamReader"/> that contains the file.</param>
        /// <returns><c>true</c> if the file is valid according to the rules; otherwise, <c>false</c>.</returns>
        bool IsValid(StreamReader sr);

        /// <summary>
        ///     Determine if the given <see cref="T:System.IO.Stream"/>
        /// represents a file which conforms to all rules.
        /// </summary>
        /// <param name="sr">A <see cref="T:System.IO.Stream"/> that contains the file.</param>
        /// <returns><c>true</c> if the file is valid according to the rules; otherwise, <c>false</c>.</returns>
        bool IsValid(Stream s);
    }
}
