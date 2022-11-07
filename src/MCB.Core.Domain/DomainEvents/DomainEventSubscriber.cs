using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Domain.Entities.Abstractions.DomainEvents;
using System.Collections.Concurrent;

namespace MCB.Core.Domain.DomainEvents;

public class DomainEventSubscriber
    : IDomainEventSubscriber
{
    // Fields
    private readonly ConcurrentQueue<IDomainEvent> _domainEventCollection;

    // Properties
    public IEnumerable<IDomainEvent> DomainEventCollection => _domainEventCollection.AsEnumerable();

    // Constructors
    public DomainEventSubscriber()
    {
        _domainEventCollection = new ConcurrentQueue<IDomainEvent>();
    }

    // Public Methods
    public Task HandlerAsync(IDomainEvent subject, CancellationToken cancellationToken)
    {
        _domainEventCollection.Enqueue(subject);
        return Task.CompletedTask;
    }
}
