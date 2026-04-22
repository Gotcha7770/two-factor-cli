using Spectre.Console;
using Spectre.Console.Cli;
using Two.Factor.Cli;

var app = new CommandApp();

app.Configure(config =>
{
#if DEBUG
    config.PropagateExceptions();
    config.ValidateExamples();
#endif

    config.Settings.Registrar.RegisterInstance<IStore>(new FileSystemStore());
    config.Settings.Registrar.RegisterInstance(AnsiConsole.Console);

    config.SetExceptionHandler((ex, _) =>
    {
        AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);

        return -99;
    });
});

return app.Run(args);