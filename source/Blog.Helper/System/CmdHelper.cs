using System;
using System.Diagnostics;

namespace Blog.Helper.System
{
    public static class CmdHelper
    {
        public static void Command(string command)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            proc.StartInfo.Arguments = "/c \" " + command + " \"";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();

            while (!proc.StandardOutput.EndOfStream)
                Console.WriteLine(proc.StandardOutput.ReadLine());
        }

        public static void Bash(string command)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "/bin/bash";
            proc.StartInfo.Arguments = "-c \" " + command + " \"";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();

            while (!proc.StandardOutput.EndOfStream)
                Console.WriteLine(proc.StandardOutput.ReadLine());
        }
    }
}
