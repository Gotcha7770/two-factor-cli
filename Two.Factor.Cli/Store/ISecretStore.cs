namespace Two.Factor.Cli.Store;

public interface ISecretStore
{
    Task Save(string key, string secret, CancellationToken cancellationToken = default);
    Task<TotpEntry> Get(string key, CancellationToken cancellationToken = default);
    IAsyncEnumerable<TotpEntry> GetAll();
    Task Remove(string key, CancellationToken cancellationToken = default);
}