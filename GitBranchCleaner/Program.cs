using System;

namespace GitBranchCleaner
{
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.Diagnostics;

    class Program
    {
        static void Main(string[] args)
        {
            var branches = GetBranches(RunCommand("git", "branch -r --merged master", @"C:\www\komplett.caas"));

            foreach (var branchName in branches)
            {
               Console.WriteLine(branchName);
               var res = RunCommand2("git", $"git push origin --delete {branchName}", @"C:\www\komplett.caas");
               Console.WriteLine(res);
            }
            
            Console.ReadKey();
        }

        private static IEnumerable<string> RunCommand(string cmd, string args, string workingDir)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = cmd,
                Arguments = args,
                WorkingDirectory = workingDir,
                ErrorDialog = true,
                RedirectStandardOutput = true
            };

            var process = Process.Start(startInfo);

            while (!process.StandardOutput.EndOfStream)
            {
                yield return process.StandardOutput.ReadLine();
            }
        }

        private static string RunCommand2(string cmd, string args, string workingDir)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = cmd,
                Arguments = args,
                WorkingDirectory = workingDir,
                ErrorDialog = true,
                RedirectStandardOutput = true
            };

            var process = Process.Start(startInfo);
            process.WaitForExit();

            while (!process.StandardOutput.EndOfStream)
            {
                return process.StandardOutput.ReadLine();
            }
        }

        private static IEnumerable<string> GetBranches(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                if (line.Contains("master", StringComparison.InvariantCultureIgnoreCase) ||
                    line.Contains("head", StringComparison.InvariantCultureIgnoreCase) ||
                    line.Contains("pointrelease", StringComparison.InvariantCultureIgnoreCase))
                {

                }
                else
                {
                    yield return line.Replace("origin/", string.Empty);
                }
            }
        }
    }
}
