namespace WinStream.Core.Models;

public class Playlist
{
    public string Name
    {
        get; set;
    }
    public ICollection<Song> Songs
    {
        get; set;
    }
}