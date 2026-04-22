using System.IO.Abstractions;
using Spectre.Console;
using Spectre.Console.Cli;
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
    config.SetApplicationVersion("0.0.1");

    config.Settings.Registrar.RegisterInstance(AnsiConsole.Console);
    config.Settings.Registrar.Register<IFileSystem, FileSystem>();
    if (OperatingSystem.IsWindows())
        config.Settings.Registrar.Register<ISecretStore, DpapiSecretStore>();
    else
        throw new PlatformNotSupportedException();

    config.AddCommand<ListCommand>("list")
        .WithDescription("Получить весь список сохраненных секретов текущего пользователя");

    config.SetExceptionHandler((ex, _) =>
    {
        AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);

        return -99;
    });
});

return app.Run(args);