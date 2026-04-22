using Spectre.Console;
using Spectre.Console.Cli;

namespace Two.Factor.Cli.Commands;

public class ListCommand : AsyncCommand<ListCommand.Settings>
{
    private readonly ISecretStore _secretStore;
    private readonly IAnsiConsole _ansiConsole;

    public ListCommand(ISecretStore secretStore, IAnsiConsole ansiConsole)
    {
        _secretStore = secretStore;
        _ansiConsole = ansiConsole;
    }

    protected override async Task<int> ExecuteAsync(
        CommandContext context,
        Settings settings,
        CancellationToken cancellationToken)
    {
        await foreach (var item in _secretStore.GetAll().WithCancellation(cancellationToken))
            _ansiConsole.Markup(item.ToString());

        return 0;
    }

    public class Settings : CommandSettings
    {
    }
}