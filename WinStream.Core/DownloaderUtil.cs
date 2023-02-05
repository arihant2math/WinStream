using WinStream.Core.Services;

namespace WinStream.Core;

public class DownloaderUtil
{
    private static bool IsInstalledProperly()
    {
        return File.Exists(DownloaderService.YoutubeDlPath);
    }
    
    public static async Task EnsureCorrectInstallation()
    {
        if (!IsInstalledProperly())
        {
            Directory.CreateDirectory(DownloaderService.ExeFolder);
            var httpClient = new HttpClient();
            var resp = await httpClient.GetAsync("https://youtube-dl.org/downloads/latest/youtube-dl.exe");
            var stream = await resp.Content.ReadAsStreamAsync();
            var fileStream = File.Create(DownloaderService.YoutubeDlPath);
            stream.Seek(0, SeekOrigin.Begin);
            await stream.CopyToAsync(fileStream);
            fileStream.Close();
        }
    }
}