namespace WinStream.Core.Models;

public class Song
{
    public static readonly Song None = new(); 
    public string Name
    {
        get;
        set;
    }

    public string Composer
    {
        get;
        set;
    }
    
    public DateTime Published
    {
        get;
        set;
    }

    public string Location
    {
        get;
        set;
    }

    public string Album
    {
        get;
        set;
    }

    public string Performer
    {
        get;
        set;
    }

    public Song()
    {
        Name = "";
        Composer = "";
        Published = DateTime.Today;
        Location = "";
        Album = "";
        Performer = "";
    }

    public string ShortDescription => $"{Name} by {Composer} performed by {Performer}";

    public override string ToString() => $"{Name} {Composer} {Published} {Performer}";

    public static Song FromPath(string file)
    {
        var name = Path.GetFileName(file);
        try
        {
            var metadata = TagLib.File.Create(Path.GetFullPath(file));
            var songName = name.Replace(".mp3", "").Replace(".wav", "").Replace(".m4a", "");
            var sTitle = metadata.Tag.Title;
            if (sTitle != null)
            {
                sTitle = sTitle.Replace(" ", "");
                sTitle = sTitle.Replace("\n", "");
                if (sTitle != "")
                {
                    songName = metadata.Tag.Title;
                }
            }
            var composer = metadata.Tag.Composers.Length != 0 ? string.Join(", ", metadata.Tag.Composers) : "Unknown";

            var album = "None";
            if (!string.IsNullOrEmpty(metadata.Tag.Album))
            {
                album = metadata.Tag.Album;
            }

            var performer = composer;
            if (metadata.Tag.PerformersRole.Length != 0)
            {
                performer = string.Join(", ", metadata.Tag.PerformersRole);
            }
            else if (metadata.Tag.AlbumArtists.Length != 0)
            {
                performer = string.Join(", ", metadata.Tag.AlbumArtists);
            }
            return new Song() { Composer = composer, Name = songName, Published = DateTime.Today, Location = file, Album = album, Performer = performer };
        }
        catch (TagLib.CorruptFileException)
        {
            return new Song() { Composer = "unknown", Name = name.Replace(".mp3", "").Replace(".wav", "").Replace(".m4a", ""), Published = DateTime.Today, Location = file, Album = "Unknown", Performer = "Unknown" };
        }
    }

    public override bool Equals(object obj)
    {
        if (obj == null || this.GetType() != obj.GetType())
        {
            return false;
        }
        var s = (Song) obj;
        return s.Location == Location;
    }

    public override int GetHashCode()
    {
        return Location.GetHashCode();
    }
}