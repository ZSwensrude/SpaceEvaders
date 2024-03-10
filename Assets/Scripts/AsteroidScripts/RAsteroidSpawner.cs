using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAsteroidSpawner : MonoBehaviour
{
    
    public void GetInterTimes()
    {
        System.Diagnostics.Process process = new System.Diagnostics.Process();

        process.StartInfo.FileName = @"Rscript";
        process.StartInfo.Arguments = "AsteroidSpawner.R 10 10";
        process.StartInfo.WorkingDirectory = Application.dataPath;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        string err = process.StandardError.ReadToEnd();
        process.WaitForExit();
        Debug.Log(output);
        Debug.LogError(err);
    }

}
