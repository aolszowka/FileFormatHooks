// -----------------------------------------------------------------------
// <copyright file="StringExtensionMethods.cs" company="Ace Olszowka">
// Copyright (c) 2015 Ace Olszowka.
// </copyright>
// -----------------------------------------------------------------------

namespace FileFormatHooks
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// <see cref="T:System.String"/> extension method class.
    /// </summary>
    public static class StringExtensionMethods
    {
        /// <summary>
        /// Determines if the given string contains any of the <see cref="searchStrings"/>.
        /// </summary>
        /// <param name="source">The <see cref="T:System.String"/> to search.</param>
        /// <param name="searchStrings">The <see cref="T:System.String"/> to search for.</param>
        /// <returns><c>true</c> if the given string contains any of the search strings; otherwise, <c>false</c>.</returns>
        public static bool ContainsAny(this string source, IEnumerable<string> searchStrings)
        {
            if (searchStrings == null)
            {
                throw new ArgumentNullException("searchStrings");
            }

            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (string.IsNullOrWhiteSpace(source))
            {
                return false;
            }

            foreach (string searchString in searchStrings)
            {
                if (source.Contains(searchString))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
