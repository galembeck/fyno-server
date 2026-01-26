namespace Domain.Utils.Constants;

public sealed record Settings
{
    public string Version { get; set; }
    public string Environment { get; set; }
    public string Domain { get; set; }
    public string SystemId { get; set; }
    public int MaxPoolConnections { get; set; }
    public AuthSettings AuthSettings { get; set; }
    public JwtSettings JwtSettings { get; set; }
}
