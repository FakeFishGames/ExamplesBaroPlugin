using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PluginToolbox;

internal static class DotnetCmd
{
    private const string DesiredRuntimeVersion = "8.0.0";

    public static bool CompileProject(string projectPath, Configuration configuration, Runtime runtime)
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            ArgumentList =
            {
                "build",
                projectPath,
                "-c",
                configuration.ToString(),
                "-clp:ErrorsOnly;Summary",
                "-r",
                runtime.Identifier,
                "/p:Platform=AnyCPU",
                $"/p:RuntimeFrameworkVersion={DesiredRuntimeVersion}"
            },
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        var process = Process.Start(psi) ?? throw new Exception("Failed to start dotnet process");

        bool errorOccurred = false;

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                if (e.Data.Contains("error"))
                {
                    errorOccurred = true;
                    Logger.LogError(e.Data);
                }
                else
                {
                    Logger.Log(e.Data);
                }
            }
        };

        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                Logger.LogError(e.Data);
                errorOccurred = true;
            }
        };

        process.WaitForExit();

        if (errorOccurred)
        {
            Logger.LogError($"Failed to build {projectPath}");
        }
        else
        {
            Logger.Log($"Successfully built {projectPath}");
        }

        return !errorOccurred;
    }
}