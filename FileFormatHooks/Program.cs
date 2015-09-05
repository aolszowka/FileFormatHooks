// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ace Olszowka">
// Copyright (c) 2015 Ace Olszowka.
// </copyright>
// -----------------------------------------------------------------------

namespace FileFormatHooks
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using SharpSvn;

    /// <summary>
    /// Main entry point for the file format commit hooks.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Diagnostics.Debugger.Launch();

            SvnHookArguments ha;
            if (!SvnHookArguments.ParseHookArguments(args, SvnHookType.PreCommit, false, out ha))
            {
                Console.Error.WriteLine("Invalid Arguments Sent to FileFormatHooks Commit Hook");
                Environment.Exit(1);
            }

            if (ShouldCommitHooksRun(ha))
            {
                IEnumerable<string> addedOrModifiedFiles = SvnUtils.GetAddedOrModifiedFilesFromCommit(ha).ToArray();

                foreach (string addedOrModifiedFile in addedOrModifiedFiles)
                {
                    // Get the file extension
                    string fileExtension = Path.GetExtension(addedOrModifiedFile).ToLowerInvariant();

                    switch (fileExtension)
                    {
                        case ".cs":
                            {
                                IFileRules csharpFileRules = new CSharpFileRules();
                                SvnUtils.SvnLookEvaluateContent<bool>(ha, addedOrModifiedFile, csharpFileRules.IsValid);
                                break;
                            }
                    }
                }
            }

            Environment.Exit(1);
        }

        static bool ShouldCommitHooksRun(SvnHookArguments ha)
        {
            // By default always run the commit hooks
            bool runCommitHooks = true;

            SvnLookOrigin slo = new SvnLookOrigin(ha.RepositoryPath, ha.TransactionName);
            SvnChangeInfoEventArgs sciea;
            using (SvnLookClient slc = new SvnLookClient())
            {
                bool gotLogInfo = slc.GetChangeInfo(slo, out sciea);
                Trace.WriteLineIf(!gotLogInfo, "Failed to GetLogInfo() for Commit, Commit Hooks NOT Ran");

                // If we fail to get the log we're going to play it safe
                // and not run these commit hooks
                runCommitHooks = gotLogInfo;
            }

            if (runCommitHooks)
            {
                // If the magic keywords [ignoreall] or [ignorefileformathooks]
                // are found don't run these commit hooks.
                string logMessage = sciea.LogMessage;
                if (!string.IsNullOrEmpty(logMessage))
                {
                    IEnumerable<string> magicIgnoreKeywords = new string[] { "[ignoreall]", "[ignorefileformathooks]" };
                    runCommitHooks = !logMessage.ContainsAny(magicIgnoreKeywords);
                }
            }

            return runCommitHooks;
        }

    }
}
