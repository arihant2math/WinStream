using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace WinStream.Core;

public class Downloader
{
    private static string _exeFolder =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "WinStream");
    private static string youtubeDlPath = Path.Combine(_exeFolder, "youtube-dl.exe");
    public string Url
    {
        get;
        set;
    }

    public string Output
    {
        get;
        set;
    }

    public enum QualitySelection
    {
        
        Best,
        Worst,
        BestVideo,
        WorstVideo,
        BestAudio,
        WorstAudio
    }

    public QualitySelection Quality
    {
        get;
        set;
    }
    public int Width
    {
        get;
        set;
    }
    public int Height
    {
        get;
        set;
    }
    public int Fps
    {
        get;
        set;
    }

    public bool AudioOnly { get; set; } = false;

    public bool EmbedThumbnail { get; set; } = false;

    public enum AudioFormats
    {
        Best,
        Acc,
        Flac,
        Mp3,
        M4a,
        Opus,
        Vorbis,
        Wav
    }

    public AudioFormats? AudioFormat { get; set; } = AudioFormats.Best;

    public int AudioQuality
    {
        get;
        set;
    }

    public enum VideoFormats
    {
        Mp4,
        Flv,
        Ogg,
        Webm,
        Mkv,
        Avi
    }
    
    public VideoFormats VideoFormat
    {
        get;
        set;
    }

    private static bool IsInstalledProperly()
    {
        return File.Exists(youtubeDlPath);
    }
    
    public static async Task EnsureCorrectInstallation()
    {
        if (!IsInstalledProperly())
        {
            Directory.CreateDirectory(_exeFolder);
            var httpClient = new HttpClient();
            var resp = await httpClient.GetAsync("https://youtube-dl.org/downloads/latest/youtube-dl.exe");
            var stream = await resp.Content.ReadAsStreamAsync();
            var fileStream = File.Create(youtubeDlPath);
            stream.Seek(0, SeekOrigin.Begin);
            await stream.CopyToAsync(fileStream);
            fileStream.Close();
        }
    }

    public string GetArguments()
    {
        var args = "--prefer-ffmpeg -o " + Output; // Need --prefer-ffmpeg because we only have ffmpeg guarantied (in the future)
        if (AudioOnly)
        {
            args += " -x";
        }

        if (EmbedThumbnail)
        {
            args += " --embed-thumbnail";
        }

        if (AudioFormat != null)
        {
            var argAudioFormat = "";
            switch (AudioFormat)
            {
                case AudioFormats.Best:
                    argAudioFormat = "best";
                    break;
                case AudioFormats.Acc:
                    argAudioFormat = "acc";
                    break;
                case AudioFormats.Flac:
                    argAudioFormat = "flac";
                    break;
                case AudioFormats.M4a:
                    argAudioFormat = "m4a";
                    break;
                case AudioFormats.Mp3:
                    argAudioFormat = "mp3";
                    break;
                case AudioFormats.Opus:
                    argAudioFormat = "opus";
                    break;
                case AudioFormats.Vorbis:
                    argAudioFormat = "vorbis";
                    break;
                case AudioFormats.Wav:
                    argAudioFormat = "wav";
                    break;
                default:
                    argAudioFormat = "best";
                    break;
            }

            args += " --audio-format " + argAudioFormat;
        }
        return args + " " + Url;
    }

    public async Task Download()
    {
        if (Url != null && Output != null)
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            process.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            Debug.WriteLine("/k \"" + youtubeDlPath + " " + GetArguments() + "\"");
            process.StartInfo.Arguments = "/k \"" + youtubeDlPath + " " + GetArguments() + "\"";
            process.StartInfo.CreateNoWindow = false;
            process.EnableRaisingEvents = true;
            process.Start();
            await process.WaitForExitAsync();
            process.Close();
        }
    }
}