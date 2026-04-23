using System.IO.Abstractions;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using JetBrains.Annotations;

namespace Two.Factor.Cli.Store;

[UsedImplicitly]
[SupportedOSPlatform("windows")]
internal class DpapiSecretStore : ISecretStore
{
    private const string StoreName = "twofa";
    private const string FileName = "secrets.json";
    private static readonly byte[] Entropy = "6k0Ryx6jxV+GMGDXloT2YA=="u8.ToArray();
    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    private readonly IFileSystem _fileSystem;
    private readonly IFileInfo _file;

    public DpapiSecretStore(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var directory = fileSystem.Path.Combine(appData, StoreName);

        if (fileSystem.Directory.NotExists(directory))
        {
            fileSystem.Directory.CreateDirectory(directory);
        }

        var path = fileSystem.Path.Combine(directory, FileName);

        _file = fileSystem.FileInfo.New(path);
    }

    public async Task SaveAsync(string key, string secret, CancellationToken cancellationToken = default)
    {
        var data = await LoadAsync();

        data[key] = Encrypt(secret);

        await SaveFileAsync(data);
    }

    public async Task<TotpEntry> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        var data = await LoadAsync();

        if (!data.TryGetValue(key, out var encrypted))
            return null;

        var secret = Decrypt(encrypted);

        return new TotpEntry { Name = key, Secret = secret };
    }

    public IAsyncEnumerable<TotpEntry> GetAll()
    {
        return AsyncEnumerable.Create(Iterator);

        async IAsyncEnumerator<TotpEntry> Iterator(CancellationToken cancellation)
        {
            var data = await LoadAsync();

            foreach (var entry in data)
            {
                yield return new TotpEntry { Name = entry.Key, Secret = entry.Value };
            }
        }
    }

    private string Encrypt(string secret)
    {
        var bytes = Encoding.UTF8.GetBytes(secret);

        var protectedBytes = ProtectedData.Protect(
            bytes,
            Entropy,
            DataProtectionScope.CurrentUser);

        return Convert.ToBase64String(protectedBytes);
    }

    private string Decrypt(string cipher)
    {
        var bytes = Convert.FromBase64String(cipher);

        var unprotectedBytes = ProtectedData.Unprotect(
            bytes,
            Entropy,
            DataProtectionScope.CurrentUser);

        return Encoding.UTF8.GetString(unprotectedBytes);
    }

    private async Task<Dictionary<string, string>> LoadAsync()
    {
        if (_file.NotExists)
            return new Dictionary<string, string>();

        await using var stream = _file.OpenRead();
        var data = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream);

        return data ?? new Dictionary<string, string>();
    }

    private async Task SaveFileAsync(Dictionary<string, string> data)
    {
        var tempFile = _file.FullName + ".tmp";

        await using (var stream = _fileSystem.File.Create(tempFile))
        {
            await JsonSerializer.SerializeAsync(stream, data, SerializerOptions);
        }

        _fileSystem.File.Move(tempFile, _file.FullName, overwrite: true);
        _fileSystem.File.SetAttributes(_file.FullName, FileAttributes.Hidden);
        _file.SetAccessControl(FileSecurity.CurrentUserAccess);
    }
}