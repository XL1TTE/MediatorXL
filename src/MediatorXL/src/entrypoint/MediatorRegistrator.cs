
using System.Reflection;
using MediatorXL.Abstractions;
using MediatorXL.Middleware;
using MediatorXL.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MediatorXL.Extensions
{
    public static class MediatorRegistrator
    {
        public static IServiceCollection AddMediatorXL(this IServiceCollection services, Action<MediatorConfig>? configure = null)
        {
            var config = new MediatorConfig();
            configure?.Invoke(config);

            foreach (var assembly in config.AssembliesToScan)
            {
                MediatorReflection.FindGenericTypeDefinitions(assembly, typeof(IEventListener<>))
                .ToList()
                .ForEach(x =>
                {
                    services.AddTransient(x.@interface, x.type);
                });

                MediatorReflection.FindGenericTypeDefinitions(assembly, typeof(IRequestHandler<,>))
                .ToList()
                .ForEach(x =>
                {
                    services.AddTransient(x.@interface, x.type);
                });
            }

            services.RegisterGlobalMiddlewares(config.AssembliesToScan);

            services.AddSingleton<IMediator, MediatorXL>((serviceProvider) => new MediatorXL(
               serviceProvider,
               MiddlewareTool.GetMiddlewaresSortedByPriority(serviceProvider)
            ));

            return services;
        }
    }
}
