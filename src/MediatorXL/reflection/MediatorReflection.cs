using System.Reflection;

namespace MediatorXL.Reflection;


internal static class MediatorReflection
{
    internal record struct GenericTypeMeta(Type @interface, Type @type);

    internal static IEnumerable<GenericTypeMeta> FindGenericTypeDefinitions(Assembly assembly, Type genericType)
    {
        var types = assembly.GetTypes()
            .Where(t => IsDefinition(t))
            .SelectMany(t => t.GetInterfaces(), (@type, @interface) => new GenericTypeMeta
            (
                @interface,
                @type
            )).Where(x => IsGenericTypeDefenition(x.@interface, genericType)).AsEnumerable();

        return types;
    }


    #region Utility

    private static bool IsGenericTypeDefenition(this Type type, Type genericType)
        => type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
    private static bool IsDefinition(Type type) => !type.IsAbstract && !type.IsInterface;

    #endregion
}
