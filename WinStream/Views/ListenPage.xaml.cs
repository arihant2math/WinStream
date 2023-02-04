using System.Diagnostics;
using Windows.Media.Core;
using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml.Controls;
using WinStream.Core.Models;
using WinStream.ViewModels;

namespace WinStream.Views;

public sealed partial class ListDetailsPage : Page
{
    public ListenListDetailsViewModel ViewModel
    {
        get;
    }

    public ListDetailsPage()
    {
        ViewModel = App.GetService<ListenListDetailsViewModel>();
        InitializeComponent();
    }

    private void ListDetailsViewControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListDetailsView listDetailsView)
        {
            return;
        }

        if (listDetailsView == null)
        {
            return;
        }

        if (listDetailsView.SelectedItem is not Song song)
        {
            return;
        }

        var uri = new Uri(song.Location);
        PlayerElement.Source = MediaSource.CreateFromUri(uri);
        PlayerElement.MediaPlayer.Play();
    }
}
