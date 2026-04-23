using JetBrains.Annotations;
using Spectre.Console;
using Spectre.Console.Cli;
using Two.Factor.Cli.Store;

namespace Two.Factor.Cli.Commands;

[UsedImplicitly]
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
        var items = _secretStore.GetAll()
            .Select(x => x.Name)
            .DefaultIfEmpty("- No items stored -");

        await foreach (var item in items.WithCancellation(cancellationToken))
        {
            _ansiConsole.Markup($"[green]{item}[/]");
        }

        return 0;
    }

    public class Settings : CommandSettings
    {
    }
}