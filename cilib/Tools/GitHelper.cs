using System;
using System.Diagnostics;
using System.IO;

public class GitHelper
{
    private readonly ShellHelper shellHelper;

    public GitHelper(
        ShellHelper shellHelper
    )
    {
        this.shellHelper = shellHelper;
    }


    public void CloneOrPull(Uri gitUri, string directory)
    {
        if (!Directory.Exists(directory))
            Clone(gitUri, directory);
        else
            Pull(directory);
    }

    public void Pull(string directory)
    {
        var cmd = $"git --git-dir \"{directory}\" pull";
        shellHelper.Bash(cmd);
    }

    public void Clone(Uri gitUri, string directory)
    {
        var cmd = "git clone " + "\"" + gitUri.ToString() + "\" \"" + directory + "\"";
        shellHelper.Bash(cmd);
    }
    
}