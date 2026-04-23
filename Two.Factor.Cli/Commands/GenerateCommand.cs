using System.ComponentModel;
using JetBrains.Annotations;
using OtpNet;
using Spectre.Console;
using Spectre.Console.Cli;
using Two.Factor.Cli.Store;

namespace Two.Factor.Cli.Commands;

[UsedImplicitly]
public class GenerateCommand : AsyncCommand<GenerateCommand.Settings>
{
    private readonly ISecretStore _secretStore;
    private readonly IAnsiConsole _ansiConsole;

    public GenerateCommand(ISecretStore secretStore, IAnsiConsole ansiConsole)
    {
        _secretStore = secretStore;
        _ansiConsole = ansiConsole;
    }

    protected override async Task<int> ExecuteAsync(
        CommandContext context,
        Settings settings,
        CancellationToken cancellationToken)
    {
        var entry = await _secretStore.GetAsync(settings.Name, cancellationToken);

        byte[] secretBytes = Base32Encoding.ToBytes(entry.Secret);
        var totp = new Totp(secretBytes);
        var code = totp.ComputeTotp();

        _ansiConsole.MarkupLine($"[green]{code}[/]");

        return 0;
    }

    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<name>")]
        [Description("Name of secret in store")]
        public string Name { get; init; }
    }
}