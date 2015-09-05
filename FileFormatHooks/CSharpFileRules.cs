// -----------------------------------------------------------------------
// <copyright file="CSharpProjectFileFormatRules.cs" company="Ace Olszowka">
// Copyright (c) 2015 Ace Olszowka.
// </copyright>
// -----------------------------------------------------------------------

namespace FileFormatHooks
{
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A FileRules validator for CSharp files (*.cs)
    /// </summary>
    public class CSharpFileRules : IFileRules
    {
        private const byte LF = 0x0A;
        private const byte CR = 0x0D;
        private const byte TAB = 0x09;
        private const byte SPACE = 0x20;

        /// <summary>
        /// Determines if a given stream (which is expected to represent a C# file) conforms to a limited subset of our formatting rules.
        /// </summary>
        /// <param name="sr">A StreamReader C# file.</param>
        /// <returns><c>true</c> if the file is determined to comply with the subset of our formatting rules; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This is pretty hairy, simply because you have a forward only stream you're working with.</remarks>
        public bool IsValid(StreamReader sr)
        {
            if (sr == null)
            {
                throw new ArgumentNullException("sr");
            }

            return this.IsValid(sr.BaseStream);
        }

        /// <summary>
        /// Determines if a given stream (which is expected to represent a C# file) conforms to a limited subset of our formatting rules.
        /// </summary>
        /// <param name="s">A stream of bytes representing a C# file.</param>
        /// <returns><c>true</c> if the file is determined to comply with the subset of our formatting rules; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This is pretty hairy, simply because you have a forward only stream you're working with.</remarks>
        public bool IsValid(Stream s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            return UTFUtilities.ContainsUTF8BOM(s) && UsesCorrectLineEndingsAndIndentationCharacter(s);
        }

        /// <summary>
        /// Given a Stream determine if it:
        ///  * Uses the correct line endings (CRLF)
        ///  * Uses the correct indentation character (TAB)
        ///  * Does not contain trailing whitespace (TAB or Spaces)
        /// </summary>
        /// <param name="s">The stream to evaluate.</param>
        /// <returns><c>true</c> if the given stream matches the above criteria; otherwise, <c>false</c>.</returns>
        private static bool UsesCorrectLineEndingsAndIndentationCharacter(Stream s)
        {
            int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];
            int previousByte = int.MinValue;
            int bytesRead = 0;

            while ((bytesRead = s.Read(buffer, 0, buffer.Length)) > 0)
            {
                for (int i = 0; i < bytesRead; i++)
                {
                    byte currentByte = buffer[i];

                    switch (currentByte)
                    {
                        case LF:
                            {
                                if (previousByte != CR)
                                {
                                    // Line Feed can only follow a CR
                                    Console.Error.WriteLine("LF detected with no preceding CR.");
                                    return false;
                                }
                                break;
                            }
                        case CR:
                            {
                                if (previousByte == CR)
                                {
                                    // Two Carriage Returns in a Row
                                    Console.Error.WriteLine("CR followed by a CR detected.");
                                    return false;
                                }
                                else if (previousByte == TAB || previousByte == SPACE)
                                {
                                    // Insignificant Whitespace
                                    Console.Error.WriteLine("Insignificant whitespace detected.");
                                    return false;
                                }
                                break;
                            }
                        case SPACE:
                            {
                                if (previousByte == LF)
                                {
                                    // You must use TAB for Indentation
                                    Console.Error.WriteLine("SPACE detected at the start of a line (following a LF), you must use TAB for indentation.");
                                    return false;
                                }
                                else if (previousByte == TAB)
                                {
                                    // A TAB followed by a SPACE usually indicates an indentation attempt
                                    Console.Error.WriteLine("SPACE detected following a TAB, this usually indicates an invalid indentation attempt. Use TAB for indentation.");
                                    return false;
                                }
                                break;
                            }
                        case TAB:
                            {
                                if (previousByte == SPACE)
                                {
                                    // A SPACE followed by a TAB usually indicates an indentation attempt
                                    Console.Error.WriteLine("TAB detected following a SPACE, this usually indicates an invalid indentation attempt. Use TAB for indentation.");
                                    return false;
                                }
                                break;
                            }
                        default:
                            {
                                if (previousByte == CR && currentByte != LF)
                                {
                                    // ONLY LF should follow carriage return
                                    Console.Error.WriteLine("No LF detected after a CR, use CRLF for line endings.");
                                    return false;
                                }
                                break;
                            }
                    } // End Switch

                    // Set the previous byte
                    previousByte = currentByte;
                } // End For
            } // End While

            // If you reach this point the file contained valid
            // Line Endings and Indentation characters
            return true;
        }
    }
}
