using System.IO.Abstractions;
using System.Reflection;
using Spectre.Console;
using Spectre.Console.Cli;
using Two.Factor.Cli;
using Two.Factor.Cli.Commands;
using Two.Factor.Cli.Store;

var app = new CommandApp();

app.Configure(config =>
{
#if DEBUG
    config.PropagateExceptions();
    config.ValidateExamples();
#endif

    config.SetApplicationName("2fa");
    var version = Assembly.GetExecutingAssembly().GetVersion();
    config.SetApplicationVersion(version);

    config.Settings.Registrar.RegisterInstance(AnsiConsole.Console);
    config.Settings.Registrar.Register<IFileSystem, FileSystem>();
    if (OperatingSystem.IsWindows())
        config.Settings.Registrar.Register<ISecretStore, DpapiSecretStore>();
    else
        throw new PlatformNotSupportedException();

    config.AddCommand<AddCommand>("add")
        .WithDescription("Adds a new totp entry to the store.")
        .WithExample("add", "github", "PEPTHLLCKWFSFJVCMX7QNHITRM2PDS3G");
    config.AddCommand<ListCommand>("list")
        .WithDescription("Get the full list of saved totp entries for the current user.");
    config.AddCommand<GenerateCommand>("gen")
        .WithDescription("Generates totp code for given key")
        .WithExample("gen", "github");

    config.SetExceptionHandler((ex, _) =>
    {
        AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);

        return -99;
    });
});

return app.Run(args);