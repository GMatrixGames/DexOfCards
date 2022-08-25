using System.IO;
using System.Reflection;

namespace DexOfCards.Utilities;

public static class FilePaths
{
    public static readonly string Resources = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)!, "Resources");
}