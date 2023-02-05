using WinStream.Core.Contracts.Services;
using WinStream.Core.Models;

namespace WinStream.Core.Services;

public class SongDataService : ISongDataService
{
    private List<Song> _allSongs;

    private List<string> _paths;


    public SongDataService()
    {
        _paths = new List<string>();
    }

    private IEnumerable<Song> AllSongs()
    {
        IEnumerable<Song> allSongs = new List<Song>();
        return (from p in _paths where Directory.Exists(p) select Directory.GetFiles(p)).Aggregate(allSongs, (current1, fileEntries) => fileEntries.Where(file => file.EndsWith(".mp3") || file.EndsWith(".wav") || file.EndsWith(".m4a")).Aggregate(current1, (current, file) => current.Concat(new[] { Song.FromPath(file) })));
    }
    
    public async Task<IEnumerable<Song>> GetListDetailsDataAsync(List<string> paths)
    {
        _paths = paths;
        _allSongs ??= new List<Song>(AllSongs());

        await Task.CompletedTask;
        return _allSongs;
    }
}