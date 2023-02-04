using System.Diagnostics;

namespace WinStream.Core.Services;

public class DownloaderService
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