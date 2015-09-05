// -----------------------------------------------------------------------
// <copyright file="SvnUtils.cs" company="Ace Olszowka">
// Copyright (c) 2015 Ace Olszowka.
// </copyright>
// -----------------------------------------------------------------------

namespace FileFormatHooks
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using SharpSvn;

    public static class SvnUtils
    {
        public static IEnumerable<string> GetAddedOrModifiedFilesFromCommit(SvnHookArguments ha)
        {
            SvnLookOrigin slo = new SvnLookOrigin(ha.RepositoryPath, ha.TransactionName);
            Collection<SvnChangedEventArgs> changedItems = new Collection<SvnChangedEventArgs>();
            using (SvnLookClient slc = new SvnLookClient())
            {
                bool gotChangedItems = slc.GetChanged(slo, out changedItems);
                Trace.WriteLineIf(!gotChangedItems, "Failed to GetChanged() for Commit, Commit Hooks NOT Ran");
            }

            // Filter to only files that were changed (basically anything that wasn't deleted)
            return changedItems
                    .Where(current => current.NodeKind == SvnNodeKind.File && current.Action != SvnChangeAction.Delete)
                    .Select(current => current.Path);
        }

        public static TResult SvnLookEvaluateContent<TResult>(SvnHookArguments ha, string targetPath, Func<StreamReader, TResult> stdOutEvaluator)
        {
            using (Process svnLookProcess = new Process())
            {
                svnLookProcess.StartInfo = new ProcessStartInfo(@"C:\DevApps\Development\svn\bin\svnlook.exe", _BuildSVNStartInfoArguments(ha.RepositoryPath, ha.TransactionName, targetPath));
                svnLookProcess.StartInfo.UseShellExecute = false;
                svnLookProcess.StartInfo.RedirectStandardOutput = true;
                svnLookProcess.Start();

                TResult result = stdOutEvaluator(svnLookProcess.StandardOutput);

                svnLookProcess.WaitForExit();
                return result;
            }
        }

        internal static string _BuildSVNStartInfoArguments(string pathToRepository, string transaction, string pathToFile)
        {
            return string.Format("cat \"{0}\" \"{1}\" -t \"{2}\"", pathToRepository, pathToFile, transaction);
        }
    }
}
