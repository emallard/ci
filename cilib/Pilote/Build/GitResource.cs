


using System.Diagnostics;

public class GitResource
{
    private readonly ShellHelper shellHelper;

    public GitResource(
        ShellHelper shellHelper
    )
    {
        this.shellHelper = shellHelper;
    }

    public void Pull(string directory)
    {
        var cmd = $"git --git-dir=\"{directory}\" pull";
        shellHelper.Bash(cmd);
    }

    public void Clone(string repository, string directory)
    {
        var cmd = "git clone " + "\"" + repository + "\" \"" + directory + "\"";
        shellHelper.Bash(cmd);
    }
    
}