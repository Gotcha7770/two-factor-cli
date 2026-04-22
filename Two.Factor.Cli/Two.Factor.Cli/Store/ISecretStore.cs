namespace Two.Factor.Cli.Store;

public interface ISecretStore
{
    Task SaveAsync(string key, string secret);
    Task<TotpEntry> GetAsync(string key);
    IAsyncEnumerable<TotpEntry> GetAll();
}