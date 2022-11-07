using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Domain.DomainEvents.Interfaces;
using MCB.Core.Domain.Entities.Abstractions.DomainEvents;

namespace MCB.Core.Domain.DomainEvents;

internal class DomainEventPublisher
    : IDomainEventPublisher
{
    // Fields
    private readonly IDomainEventPublisherInternal _DomainEventPublisherInternal;

    // Constructors
    internal DomainEventPublisher(IDomainEventPublisherInternal DomainEventPublisherInternal)
    {
        _DomainEventPublisherInternal = DomainEventPublisherInternal;
    }

    // Public Methods
    public Task PublishDomainEventAsync(IDomainEvent DomainEvent, CancellationToken cancellationToken)
    {
        return _DomainEventPublisherInternal.PublishAsync(DomainEvent, cancellationToken);
    }
}
