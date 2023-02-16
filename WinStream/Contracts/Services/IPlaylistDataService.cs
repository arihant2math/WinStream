using WinStream.Core.Models;

namespace WinStream.Contracts.Services;

public interface IPlaylistDataService
{
    Task InitializeAsync();

    Task UpdatePlaylists();

    List<Playlist> GetPlaylists();

    Task AddPlaylist(string name);
    Task AddPlaylist(Playlist p);

    Task DeletePlaylist(string name);
}