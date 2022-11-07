using MCB.Core.Domain.Entities.Abstractions.DomainEvents;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;

namespace MCB.Core.Domain.DomainEvents.Factories;

public abstract class DomainEventFactoryBase
{
    // Properties
    protected IDateTimeProvider DateTimeProvider { get; }

    // Constructors
    protected DomainEventFactoryBase(IDateTimeProvider dateTimeProvider)
    {
        DateTimeProvider = dateTimeProvider;
    }

    // Protected Methods
    protected (Guid id, DateTimeOffset timestamp, string domainEventType) GetBaseEventFields<TDomainEvent>()
        where TDomainEvent : IDomainEvent
    {
        return (
            id: Guid.NewGuid(),
            timestamp: DateTimeProvider.GetDate(),
            domainEventType: typeof(TDomainEvent).FullName ?? string.Empty
        );
    }
}
