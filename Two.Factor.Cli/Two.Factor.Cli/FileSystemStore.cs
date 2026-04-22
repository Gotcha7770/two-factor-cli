namespace Two.Factor.Cli;

internal class FileSystemStore : IStore
{
    public async IAsyncEnumerable<string> GetAll()
    {
        await Task.CompletedTask;
        yield break;
    }
}