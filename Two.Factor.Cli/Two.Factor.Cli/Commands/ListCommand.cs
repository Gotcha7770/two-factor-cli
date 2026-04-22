using Spectre.Console;
using Spectre.Console.Cli;

namespace Two.Factor.Cli.Commands;

public class ListCommand : AsyncCommand<ListCommand.Settings>
{
    private readonly IStore _store;
    private readonly IAnsiConsole _ansiConsole;

    public ListCommand(IStore store, IAnsiConsole ansiConsole)
    {
        _store = store;
        _ansiConsole = ansiConsole;
    }

    protected override async Task<int> ExecuteAsync(
        CommandContext context,
        Settings settings,
        CancellationToken cancellationToken)
    {
        await foreach (var item in _store.GetAll().WithCancellation(cancellationToken))
            _ansiConsole.Markup(item);

        return 0;
    }

    public class Settings : CommandSettings
    {
    }
}