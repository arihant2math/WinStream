using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using WinStream.Core.Models;

namespace WinStream.Views;

public sealed partial class PlaylistDetailControl : UserControl
{
    public Playlist? ListDetailsMenuItem
    {
        get => GetValue(ListDetailsMenuItemProperty) as Playlist;
        set => SetValue(ListDetailsMenuItemProperty, value);
    }

    public MediaPlayerElement MediaPlayer;

    public Song? Playing;

    public static readonly DependencyProperty ListDetailsMenuItemProperty = 
        DependencyProperty.Register("ListDetailsMenuItem", typeof(Playlist), 
            typeof(PlaylistDetailControl), 
            new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));
    
    public PlaylistDetailControl()
    {
        InitializeComponent();
    }

    private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PlaylistDetailControl control)
        {
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }

    private void SongSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        
    }
}