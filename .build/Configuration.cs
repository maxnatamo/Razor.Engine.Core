using System.ComponentModel;
using Nuke.Common.Tooling;

[TypeConverter(typeof(TypeConverter<Configuration>))]
public class Configuration : Enumeration
{
    public static Configuration Debug = new Configuration { Value = nameof(Debug) };
    public static Configuration Release = new Configuration { Value = nameof(Release) };

    /// <summary>
    /// Whether the current configuration is set to Debug.
    /// </summary>
    public bool IsDebug => Equals(Debug);

    /// <summary>
    /// Whether the current configuration is set to Release.
    /// </summary>
    public bool IsRelease => Equals(Release);

    public static implicit operator string(Configuration configuration)
    {
        return configuration.Value;
    }
}