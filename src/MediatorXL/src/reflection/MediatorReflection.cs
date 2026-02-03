using System.Reflection;

namespace MediatorXL.Reflection;


internal static class MediatorReflection
{
    internal record struct GenericTypeMeta(Type @interface, Type @type);

    internal static IEnumerable<GenericTypeMeta> FindGenericInterfaceDefenitions(Assembly assembly, Type intefaceType)
    {
        var types = assembly.GetTypes()
            .Where(t => IsDefinition(t))
            .SelectMany(t => t.GetInterfaces(), (@type, @interface) => new GenericTypeMeta
            (
                @interface,
                @type
            )).Where(x => IsGenericTypeDefenition(x.@interface, intefaceType)).AsEnumerable();

        return types;
    }
    internal static IEnumerable<Type> FindSubclassesOf(Assembly assembly, Type type)
    {
        var types = assembly.GetTypes()
            .Where(t => IsDefinition(t) && t.IsSubclassOf(type));

        return types;
    }


    #region Utility

    private static bool IsGenericTypeDefenition(this Type type, Type genericType)
        => type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
    private static bool IsDefinition(Type type) => !type.IsAbstract && !type.IsInterface;

    #endregion
}
