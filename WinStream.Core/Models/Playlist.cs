namespace WinStream.Core.Models;

public class Playlist
{
    public string Name
    {
        get; set;
    }
    public List<Song> Songs
    {
        get; set;
    }

    public Song Selected
    {
        get;
        set;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj == null || this.GetType() != obj.GetType())
        {
            return false;
        }
        var p = (Playlist) obj;
        return p.Name == Name;
    }
}