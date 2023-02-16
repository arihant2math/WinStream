using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using WinStream.Contracts.Services;
using WinStream.Contracts.ViewModels;
using WinStream.Core.Models;

namespace WinStream.ViewModels;

public class PlaylistListDetailsViewModel : ObservableRecipient, INavigationAware
{
    private readonly IPlaylistDataService _playlistDataService;
    private Playlist? _selected;

    public Playlist? Selected
    {
        get => _selected;
        set => SetProperty(ref _selected, value);
    }
    public ObservableCollection<Playlist> Playlists { get; private set; } = new ();

    public PlaylistListDetailsViewModel(IPlaylistDataService playlistDataService)
    {
        _playlistDataService = playlistDataService;
    }
    
    public async void OnNavigatedTo(object parameter)
    {
        await _playlistDataService.UpdatePlaylists();
        var data = _playlistDataService.GetPlaylists();
        Playlists = new ObservableCollection<Playlist>(data);
    }

    public void OnNavigatedFrom()
    {
    }

    public async Task AddPlaylist(string name)
    {
        await _playlistDataService.UpdatePlaylists();
        await _playlistDataService.AddPlaylist(name);
        await _playlistDataService.UpdatePlaylists();
        var data = _playlistDataService.GetPlaylists();
        Playlists = new ObservableCollection<Playlist>(data);
    }
    
    public async Task DeletePlaylist(string name)
    {
        await _playlistDataService.UpdatePlaylists();
        await _playlistDataService.DeletePlaylist(name);
        await _playlistDataService.UpdatePlaylists();
        var data = _playlistDataService.GetPlaylists();
        Playlists = new ObservableCollection<Playlist>(data);
    }
}
