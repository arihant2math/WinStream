using WinStream.Contracts.Services;
using WinStream.Core.Models;

namespace WinStream.Services;

public class PlaylistDataService
{
    private const string SettingsKey = "playlists";
    ILocalSettingsService _localSettingsService;
    private List<Playlist> _playlists;
    
    public PlaylistDataService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task InitializeAsync()
    {
        _playlists = await LoadPlaylistsFromSettingsAsync();
        await Task.CompletedTask;
    }
    
    public List<Playlist> GetPlaylists()
    {
        return new List<Playlist>();
    }
    
    private async Task<List<Playlist>> LoadPlaylistsFromSettingsAsync()
    {
        var result = await _localSettingsService.ReadSettingAsync<string>(SettingsKey);
        if (result != null)
        {
            var split = result.Split("\n");
            var playlists = new List<Playlist>();
            foreach (var playlist in split)
            {
                var songs = playlist.Split("\t");
                var p = new Playlist();
                foreach (var path in songs)
                {
                    p.Songs.Add(Song.FromPath(path));
                }
            }
        }
        return new List<Playlist>();
    }
}