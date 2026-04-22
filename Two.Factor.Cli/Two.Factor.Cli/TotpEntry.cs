namespace Two.Factor.Cli;

public record TotpEntry
{
    public string Name { get; init; }
    public string Secret { get; init; }
}