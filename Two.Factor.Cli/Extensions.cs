using System.IO.Abstractions;
using System.Reflection;

namespace Two.Factor.Cli;

public static class Extensions
{
    extension(IDirectory instance)
    {
        public bool NotExists(string path) => !instance.Exists(path);
    }

    extension(IFileInfo instance)
    {
        public bool NotExists => !instance.Exists;
    }

    extension(AsyncEnumerable)
    {
        public static IAsyncEnumerable<T> Create<T>(Func<CancellationToken, IAsyncEnumerator<T>> factory)
        {
            return new AnonymousAsyncEnumerable<T>(factory);
        }
    }

    extension(Assembly assembly)
    {
        public string GetVersion()
        {
            var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

            return attribute?.InformationalVersion ?? "0.0.1";
        }
    }
}

internal sealed class AnonymousAsyncEnumerable<T> : IAsyncEnumerable<T>
{
    private readonly Func<CancellationToken, IAsyncEnumerator<T>> _factory;

    public AnonymousAsyncEnumerable(Func<CancellationToken, IAsyncEnumerator<T>> factory) => _factory = factory;

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken)
    {
        cancellationToken
            .ThrowIfCancellationRequested(); // NB: [LDM-2018-11-28] Equivalent to async iterator behavior.

        return _factory(cancellationToken);
    }
}