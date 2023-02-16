using System.Collections.ObjectModel;
using Windows.Media.Playback;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using WinStream.Contracts.Services;
using WinStream.Contracts.ViewModels;
using WinStream.Core.Contracts.Services;
using WinStream.Core.Models;

namespace WinStream.ViewModels;

public class ListenListDetailsViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISongDataService _songDataService;
    private readonly IPlaylistDataService _playlistDataService;
    private readonly IDataSourceSelectorService _dataSourceSelectorService;
    private Song? _selected;

    public Song? Selected
    {
        get => _selected;
        set => SetProperty(ref _selected, value);
    }

    public ObservableCollection<Song> Songs { get; private set; } = new ();

    public ListenListDetailsViewModel(ISongDataService songDataService, IPlaylistDataService playlistDataService, IDataSourceSelectorService dataSourceSelectorService)
    {
        _songDataService = songDataService;
        _playlistDataService = playlistDataService;
        _dataSourceSelectorService = dataSourceSelectorService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Songs.Clear();
        var paths = _dataSourceSelectorService.DataSources;
        var data = await _songDataService.GetListDetailsDataAsync(paths);

        foreach (var item in data)
        {
            Songs.Add(item);
        }
    }

    public StackPanel GetPlaylistStackPanel()
    {
        var stackPanel = new StackPanel
        {
            Orientation = Orientation.Vertical
        };
        foreach (var playlist in _playlistDataService.GetPlaylists())
        {
            stackPanel.Children.Add(new CheckBox
            {
                Content = playlist.Name,
                IsChecked = playlist.Songs.Contains(Selected)
            });
        }
        return stackPanel;
    }
    
    public void OnNavigatedFrom()
    {
    }

    public void EnsurePlaylistContain(string name, bool value, Song song)
    {
        var playlist = _playlistDataService.GetPlaylists().Find(p => p.Name == name);
        if (playlist == null)
        {
            return;
        }

        if (playlist.Songs.Contains(song) && !value)
        {
            playlist.Songs.Remove(song);
            _playlistDataService.DeletePlaylist(playlist.Name);
            _playlistDataService.AddPlaylist(playlist);
        }
        else if (!playlist.Songs.Contains(song) && value)
        {
            playlist.Songs.Add(song);
            _playlistDataService.DeletePlaylist(playlist.Name);
            _playlistDataService.AddPlaylist(playlist);
        }
    }
}
