// -----------------------------------------------------------------------
// <copyright file="UTFUtilities.cs" company="Ace Olszowka">
// Copyright (c) 2015 Ace Olszowka.
// </copyright>
// -----------------------------------------------------------------------

namespace FileFormatHooks
{
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    ///     Utility class for storing functions for dealing with Unicode
    /// Transformation Format (UTF) files.
    /// </summary>
    public static class UTFUtilities
    {
        /// <summary>
        /// The UTF-8 BOM
        /// </summary>
        public static byte[] UTF8BOM = new byte[] { 0xEF, 0xBB, 0xBF };

        /// <summary>
        /// Given a Stream determine if it contains the UTF-8 BOM.
        /// </summary>
        /// <param name="s">The stream to evaluate.</param>
        /// <returns><c>true</c> if the stream starts with the UTF-8 BOM; otherwise, <c>false</c>.</returns>
        /// <remarks>
        ///     Be careful to only call this function at the beginning of the
        /// stream (where the BOM should live), additionally this will move the
        /// given stream by 3 bytes.
        /// </remarks>
        public static bool ContainsUTF8BOM(Stream s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            byte[] possibleBOM = new byte[3];
            int bytesRead = s.Read(possibleBOM, 0, possibleBOM.Length);

            if (bytesRead == 3)
            {
                return possibleBOM.SequenceEqual(UTF8BOM);
            }
            else
            {
                // We weren't even able to read 3 bytes, probably an empty file?
                return false;
            }
        }
    }
}
