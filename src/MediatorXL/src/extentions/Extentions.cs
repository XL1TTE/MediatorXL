
using System.Reflection;
using MediatorXL.Abstractions;
using MediatorXL.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MediatorXL.Extensions
{
    public class MediatorConfig
    {
        public IEnumerable<Assembly> AssembliesWithHandlers { get; set; } = Array.Empty<Assembly>();
    }

    public static class Extensions
    {
        public static IServiceCollection AddMediatorXL(this IServiceCollection services, Action<MediatorConfig>? configure = null)
        {
            var config = new MediatorConfig();
            configure?.Invoke(config);

            foreach (var assembly in config.AssembliesWithHandlers)
            {
                MediatorReflection.FindGenericTypeDefinitions(assembly, typeof(IHandler<>))
                .ToList()
                .ForEach(x =>
                {
                    services.AddTransient(x.@interface, x.type);
                });

                MediatorReflection.FindGenericTypeDefinitions(assembly, typeof(IHandler<,>))
                .ToList()
                .ForEach(x =>
                {
                    services.AddTransient(x.@interface, x.type);
                });
            }

            services.AddSingleton<IMediator, MediatorXL>();

            return services;
        }
    }
}
