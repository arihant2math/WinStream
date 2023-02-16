using Windows.Media.Playback;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinStream.Core.Models;
using WinStream.ViewModels;

namespace WinStream.Views;

public sealed partial class PlaylistPage : Page
{
    public PlaylistListDetailsViewModel ViewModel
    {
        get;
    }

    public PlaylistPage()
    {
        ViewModel = App.GetService<PlaylistListDetailsViewModel>();
        InitializeComponent();
    }
    
    private void ListDetailsViewControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListDetailsView listDetailsView)
        {
            return;
        }
        if (listDetailsView.SelectedItem is not Playlist playlist)
        {
            return;
        }

        if (playlist.Songs.Count < 0)
        {
            return;
        }

        var mediaPlaybackList = new MediaPlaybackList();

        mediaPlaybackList.CurrentItemChanged += MediaPlaybackList_CurrentItemChanged;
        mediaPlaybackList.ItemOpened += MediaPlaybackList_ItemOpened;
        mediaPlaybackList.ItemFailed += MediaPlaybackList_ItemFailed;
        mediaPlaybackList.MaxPlayedItemsToKeepOpen = 3;
        PlayerElement.Source = mediaPlaybackList;
        PlayerElement.MediaPlayer.Play();
    }

    private async void CreatePlaylist(object sender, RoutedEventArgs e)
    {
        var dialogContent = new TextBox()
        {
            PlaceholderText = "Playlist Name"
        };

        var dialog = new ContentDialog
        {
            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            XamlRoot = XamlRoot,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Create Playlist",
            PrimaryButtonText = "Save",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            Content = dialogContent
        };

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            await ViewModel.AddPlaylist(dialogContent.Text);
        }
    }

    private async void DeletePlaylist(object sender, RoutedEventArgs e)
    {
        if (ViewModel.Selected != null)
        {
            await ViewModel.DeletePlaylist(ViewModel.Selected.Name);
        }
    }
    
    private void MediaPlaybackList_CurrentItemChanged(MediaPlaybackList playbackList, CurrentMediaPlaybackItemChangedEventArgs args)
    {
        ViewModel.Selected.Selected = ViewModel.Selected.Songs[playbackList.Items.IndexOf(args.NewItem)];
    }
    private void MediaPlaybackList_ItemOpened(MediaPlaybackList playbackList, MediaPlaybackItemOpenedEventArgs args) 
    {
    }
    private void MediaPlaybackList_ItemFailed(MediaPlaybackList playbackList, MediaPlaybackItemFailedEventArgs args) 
    {
    }
}
