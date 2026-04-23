using System.ComponentModel;
using JetBrains.Annotations;
using Spectre.Console;
using Spectre.Console.Cli;
using Two.Factor.Cli.Store;

namespace Two.Factor.Cli.Commands;

[UsedImplicitly]
public class RemoveCommand : AsyncCommand<RemoveCommand.Settings>
{
    private readonly ISecretStore _secretStore;
    private readonly IAnsiConsole _ansiConsole;

    public RemoveCommand(ISecretStore secretStore, IAnsiConsole ansiConsole)
    {
        _secretStore = secretStore;
        _ansiConsole = ansiConsole;
    }

    protected override async Task<int> ExecuteAsync(
        CommandContext context,
        Settings settings,
        CancellationToken cancellationToken)
    {
        await _secretStore.Remove(settings.Name, cancellationToken);

        _ansiConsole.MarkupLine("[green]Key removed successfully[/]");
        return 0;
    }

    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<name>")]
        [Description("Name of secret to remove")]
        public string Name { get; init; }
    }
}