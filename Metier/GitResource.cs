


using System.Diagnostics;

public class GitResource
{

    public string GitRepository {get;set;}

    public void Get(string workingdirectory)
    {
        ProcessStartInfo gitInfo = new ProcessStartInfo();
        gitInfo.CreateNoWindow = true;
        gitInfo.RedirectStandardError = true;
        gitInfo.RedirectStandardOutput = true;
        gitInfo.FileName = "git";

        //Then create a Process to actually run the command.
        Process gitProcess = new Process();
        gitInfo.Arguments = "clone " + GitRepository; // such as "fetch orign"
        gitInfo.WorkingDirectory = workingdirectory;

        gitProcess.StartInfo = gitInfo;
        gitProcess.Start();

        string stderr_str = gitProcess.StandardError.ReadToEnd();  // pick up STDERR
        string stdout_str = gitProcess.StandardOutput.ReadToEnd(); // pick up STDOUT

        gitProcess.WaitForExit();
        gitProcess.Close();
    }
    
}