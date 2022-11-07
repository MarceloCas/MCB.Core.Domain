using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Observer;

namespace MCB.Core.Domain.DomainEvents.Interfaces
{
    internal interface IDomainEventPublisherInternal
        : IPublisher
    {
    }
}