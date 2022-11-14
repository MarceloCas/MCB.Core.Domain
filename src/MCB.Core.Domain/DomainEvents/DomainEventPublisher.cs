using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Domain.DomainEvents.Interfaces;
using MCB.Core.Domain.Entities.Abstractions.DomainEvents;

namespace MCB.Core.Domain.DomainEvents;

public class DomainEventPublisher
    : IDomainEventPublisher
{
    // Fields
    private readonly IDomainEventPublisherInternal _DomainEventPublisherInternal;

    // Constructors
    public DomainEventPublisher(IDomainEventPublisherInternal DomainEventPublisherInternal)
    {
        _DomainEventPublisherInternal = DomainEventPublisherInternal;
    }

    // Public Methods
    public Task PublishDomainEventAsync(IDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        return _DomainEventPublisherInternal.PublishAsync(domainEvent, subjectBaseType: typeof(IDomainEvent), cancellationToken);
    }
}
