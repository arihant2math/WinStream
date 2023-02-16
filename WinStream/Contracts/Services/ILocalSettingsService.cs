namespace WinStream.Contracts.Services;

public interface ILocalSettingsService
{
    Task<T?> ReadSettingAsync<T>(string key);

    Task SaveSettingAsync<T>(string key, T value);

    Task<T?> SerializedReadSettingAsync<T>(string key) where T : new();
    Task SerializedSaveSettingAsync<T>(string key, object value);
}
