using Domain.Utils.Constants;

namespace Domain.Constants;

public static class Constants
{
    public static Settings Settings { get; private set; }

    public static void SetSettings(Settings settings)
    {
        Settings = settings;
    }
}
