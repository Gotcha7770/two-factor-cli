using System.ComponentModel;
using JetBrains.Annotations;
using Spectre.Console;
using Spectre.Console.Cli;
using Two.Factor.Cli.Store;

namespace Two.Factor.Cli.Commands;

[UsedImplicitly]
public class AddCommand : AsyncCommand<AddCommand.Settings>
{
    private readonly ISecretStore _secretStore;
    private readonly IAnsiConsole _ansiConsole;

    public AddCommand(ISecretStore secretStore, IAnsiConsole ansiConsole)
    {
        _secretStore = secretStore;
        _ansiConsole = ansiConsole;
    }

    protected override async Task<int> ExecuteAsync(
        CommandContext context,
        Settings settings,
        CancellationToken cancellationToken)
    {
        await _secretStore.SaveAsync(settings.Name, settings.Secret, cancellationToken);

        _ansiConsole.MarkupLine("[green]Key saved successfully[/]");
        return 0;
    }

    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<name>")]
        [Description("Name of secret to store with")]
        public string Name { get; init; }

        [CommandArgument(1, "<secret>")]
        [Description("Secret")]
        public string Secret { get; init; }
    }
}