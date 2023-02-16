using WinStream.Contracts.Services;
using WinStream.Core.Models;

namespace WinStream.Services;

public class PlaylistDataService : IPlaylistDataService
{
    private const string SettingsKey = "playlists";
    private readonly ILocalSettingsService _localSettingsService;
    private List<Playlist> _playlists;
    
    public PlaylistDataService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
        _playlists = new List<Playlist>();
    }

    public async Task InitializeAsync()
    {
        _playlists = await LoadPlaylistsFromSettingsAsync();
        await Task.CompletedTask;
    }
    
    public List<Playlist> GetPlaylists()
    {
        return _playlists;
    }
    
    private async Task<List<Playlist>> LoadPlaylistsFromSettingsAsync()
    {
        var result = await _localSettingsService.SerializedReadSettingAsync<List<Playlist>>(SettingsKey);
        return result ?? new List<Playlist>();
    }

    private async Task SavePlaylistsAsync(List<Playlist> playlists)
    {
        await _localSettingsService.SerializedSaveSettingAsync<List<Playlist>>(SettingsKey, playlists);
    }

    public async Task UpdatePlaylists()
    {
        _playlists = await LoadPlaylistsFromSettingsAsync();
    }

    public async Task AddPlaylist(string name)
    {
        _playlists.Add(new Playlist {Name = name, Songs = new List<Song>()});
        await SavePlaylistsAsync(_playlists);
    }

    public async Task AddPlaylist(Playlist playlist)
    {
        _playlists.Add(playlist);
        await SavePlaylistsAsync(_playlists);
    }

    public async Task DeletePlaylist(string name)
    {
        _playlists.Remove(new Playlist { Name = name });
        await SavePlaylistsAsync(_playlists);
    }
}