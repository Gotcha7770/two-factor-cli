namespace Two.Factor.Cli;

public interface IStore
{
    IAsyncEnumerable<string> GetAll();
}