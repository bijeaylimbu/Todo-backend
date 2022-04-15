using System.Reflection;

namespace TodoApi.BuildingBlocks.Core;

public class ReflectionUtils
{
    public static string GetAssemblyVersion<T>()
    {
        return typeof(T).GetTypeInfo().Assembly
            .GetCustomAttributes<AssemblyInformationalVersionAttribute>()
            .FirstOrDefault()
            ?.InformationalVersion;
    }
}