using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using WinStream.Contracts.Services;
using WinStream.Contracts.ViewModels;
using WinStream.Core.Contracts.Services;
using WinStream.Core.Models;

namespace WinStream.ViewModels;

public class ListenListDetailsViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISongDataService _songDataService;
    private readonly IDataSourceSelectorService _dataSourceSelectorService;
    private Song? _selected;

    public Song? Selected
    {
        get => _selected;
        set => SetProperty(ref _selected, value);
    }

    public ObservableCollection<Song> Songs { get; private set; } = new ();

    public ListenListDetailsViewModel(ISongDataService songDataService, IDataSourceSelectorService dataSourceSelectorService)
    {
        _songDataService = songDataService;
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

    public void OnNavigatedFrom()
    {
    }
}
