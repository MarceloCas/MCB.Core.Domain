using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Domain.DomainEvents;
using MCB.Core.Domain.DomainEvents.Interfaces;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Core.Domain.Entities.Abstractions.DomainEvents;

namespace MCB.Core.Domain.DependencyInjection;

public static class Bootstrapper
{
    public static void ConfigureServices(
        IDependencyInjectionContainer dependencyInjectionContainer
    )
    {
        // Domain Events
        dependencyInjectionContainer.RegisterScoped<IDomainEventPublisherInternal, DomainEventPublisherInternal>(dependencyInjectionContainer =>
        {
            var domainEventPublisherInternal = new DomainEventPublisherInternal(dependencyInjectionContainer);

            domainEventPublisherInternal.Subscribe<IDomainEventSubscriber, IDomainEvent>();

            return domainEventPublisherInternal;
        });
        dependencyInjectionContainer.RegisterScoped<IDomainEventPublisher, DomainEventPublisher>(dependencyInjectionContainer =>
        {
            return new DomainEventPublisher(dependencyInjectionContainer.Resolve<IDomainEventPublisherInternal>());
        });
        dependencyInjectionContainer.RegisterScoped<IDomainEventSubscriber, DomainEventSubscriber>();
    }
}