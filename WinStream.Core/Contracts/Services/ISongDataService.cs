using WinStream.Core.Models;

namespace WinStream.Core.Contracts.Services;

// Remove this class once your pages/features are using your data.
public interface ISongDataService
{
    Task<IEnumerable<Song>> GetListDetailsDataAsync(List<string> paths);
}
