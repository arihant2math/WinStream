using System.Diagnostics;
using Windows.Media.Core;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
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

        if (listDetailsView.SelectedItem is not Song song)
        {
            return;
        }

        var uri = new Uri(song.Location);
        PlayerElement.Source = MediaSource.CreateFromUri(uri);
        PlayerElement.MediaPlayer.Play();
    }

    private async void AddToPlaylist(object sender, RoutedEventArgs e)
    {
        if (ViewModel.Selected == null)
        {
            return;
        }

        var dialogContent = ViewModel.GetPlaylistStackPanel();

        var dialog = new ContentDialog
        {
            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            XamlRoot = XamlRoot,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Add to Playlist",
            PrimaryButtonText = "Save",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            Content = dialogContent
        };

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            var resp = (StackPanel) dialog.Content;
            foreach (var item in resp.Children)
            {
                var check = (CheckBox) item;
                ViewModel.EnsurePlaylistContain((string) check.Content, check.IsChecked.Value, ViewModel.Selected);
            }
        }
    }
}
