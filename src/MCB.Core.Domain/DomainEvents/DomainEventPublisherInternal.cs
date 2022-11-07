using MCB.Core.Domain.DomainEvents.Interfaces;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Observer;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Observer;

namespace MCB.Core.Domain.DomainEvents;

internal class DomainEventPublisherInternal
    : PublisherBase,
    IDomainEventPublisherInternal
{
    // Constants
    public const string SUBSCRIBER_TYPE_CANNOT_INSTANCIATED_MESSAGE = "SubscriberType cannot instanciated [{0}]";

    // Fields
    private readonly IDependencyInjectionContainer _dependencyInjectionContainer;

    // Constructors
    public DomainEventPublisherInternal(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        _dependencyInjectionContainer = dependencyInjectionContainer;
    }

    protected override ISubscriber<TSubject> InstanciateSubscriber<TSubject>(Type subscriberType)
    {
        var instance = _dependencyInjectionContainer.Resolve(subscriberType);

        if (instance is null)
            throw new InvalidOperationException(string.Format(SUBSCRIBER_TYPE_CANNOT_INSTANCIATED_MESSAGE, subscriberType.FullName));

        return (ISubscriber<TSubject>)instance;
    }
}
