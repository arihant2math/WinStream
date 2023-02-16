using System.Diagnostics;
using WinStream.Core.Contracts.Services;
using WinStream.Core.Downloader;

namespace WinStream.Core.Services;

public class DownloaderService : IDownloaderService
{
    public static readonly string ExeFolder =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "WinStream");
    public static readonly string YoutubeDlPath = Path.Combine(ExeFolder, "youtube-dl.exe");
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

    public bool AudioOnly
    {
        get; 
        set;
    }

    public bool EmbedThumbnail
    {
        get;
        set;
    }

    public AudioFormats? AudioFormat { get; set; } = AudioFormats.Best;

    public int AudioQuality
    {
        get;
        set;
    }

    public VideoFormats VideoFormat
    {
        get;
        set;
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

        if (AudioFormat == null)
        {
            return args + " " + Url;
        }

        var argAudioFormat = AudioFormat switch
        {
            AudioFormats.Best => "best",
            AudioFormats.Acc => "acc",
            AudioFormats.Flac => "flac",
            AudioFormats.M4a => "m4a",
            AudioFormats.Mp3 => "mp3",
            AudioFormats.Opus => "opus",
            AudioFormats.Vorbis => "vorbis",
            AudioFormats.Wav => "wav",
            _ => "best"
        };

        args += " --audio-format " + argAudioFormat;
        return args + " " + Url;
    }

    public async Task Download()
    {
        if (Url != null && Output != null)
        {
            var path = System.Environment.GetEnvironmentVariable("path");
            if (!path.Contains(ExeFolder))
            {
                path += ";" + ExeFolder;
            }

            Environment.SetEnvironmentVariable("path", path);
            var process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            process.StartInfo.FileName = YoutubeDlPath;
            process.StartInfo.Arguments = GetArguments();
            process.StartInfo.CreateNoWindow = false;
            process.EnableRaisingEvents = true;
            process.Start();
            await process.WaitForExitAsync();
            process.Close();
        }
    }
}