using System.Diagnostics;
using UnityEngine;

public static class AppleTTS
{
    public static void Speak(string text)
    {
#if UNITY_EDITOR_OSX
        var psi = new ProcessStartInfo
        {
            FileName = "/usr/bin/say",
            Arguments = $"-v Alex \"{EscapeForShell(text)}\"",
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        Process.Start(psi);

#elif UNITY_EDITOR_WIN
        var psArgs = $"-Command \"Add-Type –AssemblyName System.Speech;" +
                     $"(New-Object System.Speech.Synthesis.SpeechSynthesizer)" +
                     $".Speak('{EscapeForPowerShell(text)}')\"";
        var psi = new ProcessStartInfo
        {
            FileName        = "powershell.exe",
            Arguments       = psArgs,
            UseShellExecute = false,
            CreateNoWindow  = true,
        };
        Process.Start(psi);

#else
        Debug.Log("[TTS] " + text);
#endif
    }

    private static string EscapeForShell(string s)
    {
        return s.Replace("\"", "\\\"");
    }

    private static string EscapeForPowerShell(string s)
    {
        return s.Replace("'", "''");
    }
}