using WinStream.Core.Contracts.Services;
using WinStream.Core.Models;
using File = TagLib.File;

namespace WinStream.Core.Services;

public class SongDataService : ISongDataService
{
    private List<Song> _allSongs;

    private List<string> Paths;


    public SongDataService()
    {
        Paths = new List<string>();
    }

    private IEnumerable<Song> AllSongs()
    {
        IEnumerable<Song> allSongs = new List<Song>();
        foreach (var p in Paths)
        {
            if (!Directory.Exists(p))
            {
                continue;
            }

            var fileEntries = Directory.GetFiles(p);
            foreach (var file in fileEntries)
            {
                if (file.EndsWith(".mp3") || file.EndsWith(".wav") || file.EndsWith(".m4a"))
                {
                    allSongs = allSongs.Concat(new[] { Song.FromPath(file) });
                }
            }
        }
        return allSongs;
    }
    
    public async Task<IEnumerable<Song>> GetListDetailsDataAsync(List<string> paths)
    {
        Paths = paths;
        _allSongs ??= new List<Song>(AllSongs());

        await Task.CompletedTask;
        return _allSongs;
    }
}