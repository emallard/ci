using System;
using System.Diagnostics;

public static class ShellHelper
{
    public static string Bash(this string cmd)
    {
        var escapedArgs = cmd.Replace("\"", "\\\"");
        
        var process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{escapedArgs}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };
        process.Start();
        string result = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return result;
    }

    public static string BashAndStdErr(this string cmd, out string stdErr)
    {
        var escapedArgs = cmd.Replace("\"", "\\\"");
        
        var process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{escapedArgs}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };
        process.Start();
        string result = process.StandardOutput.ReadToEnd();
        stdErr = process.StandardError.ReadToEnd();
        process.WaitForExit();
        return result;
    }

    public static string BashAndStdIn(this string cmd, string stdIn)
    {
        var escapedArgs = cmd.Replace("\"", "\\\"");
        
        var process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{escapedArgs}\"",
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };
        
        process.Start();
        process.StandardInput.Write(stdIn);
        string result = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return result;
    }
}