using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace MyHealth.Observations.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddComposite<TInterface, TConcrete>(this IServiceCollection services)
            where TInterface : class
            where TConcrete : class, TInterface
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var wrappedDescriptors = services.Where(s => s.ServiceType == typeof(TInterface)).ToList();
            foreach (var descriptor in wrappedDescriptors)
                services.Remove(descriptor);

            var objectFactory = ActivatorUtilities.CreateFactory(
                typeof(TConcrete),
                new[] { typeof(IEnumerable<TInterface>) });

            services.Add(ServiceDescriptor.Describe(
                typeof(TInterface),
                s => (TInterface)objectFactory(s, new[] { wrappedDescriptors.Select(d => s.CreateInstance(d)).Cast<TInterface>() }),
                wrappedDescriptors.Select(d => d.Lifetime).Max())
            );
        }

        private static object CreateInstance(this IServiceProvider services, ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationInstance != null)
                return descriptor.ImplementationInstance;

            if (descriptor.ImplementationFactory != null)
                return descriptor.ImplementationFactory(services);

            return ActivatorUtilities.GetServiceOrCreateInstance(services, descriptor.ImplementationType);
        }
    }
}
