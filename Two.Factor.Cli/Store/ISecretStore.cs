namespace Two.Factor.Cli.Store;

public interface ISecretStore
{
    Task SaveAsync(string key, string secret, CancellationToken cancellationToken = default);
    Task<TotpEntry> GetAsync(string key, CancellationToken cancellationToken = default);
    IAsyncEnumerable<TotpEntry> GetAll();
}