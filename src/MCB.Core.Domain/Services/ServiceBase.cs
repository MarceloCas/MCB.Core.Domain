﻿using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Domain.Abstractions.Repositories;
using MCB.Core.Domain.Abstractions.Services;
using MCB.Core.Domain.Entities.Abstractions;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Adapter;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Notifications.Models.Enums;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Models;

namespace MCB.Core.Domain.Services;

public abstract class ServiceBase
{
    public static readonly string AggregationRootShouldExistsInRepositoryErrorCode = nameof(AggregationRootShouldExistsInRepositoryErrorCode);
    public static readonly string AggregationRootShouldExistsInRepositoryMessage = nameof(AggregationRootShouldExistsInRepositoryMessage);
    public static readonly NotificationType AggregationRootShouldExistsInRepositoryNotificationType = NotificationType.Error;

    public static readonly string AggregationRootShouldNotExistsInRepositoryErrorCode = nameof(AggregationRootShouldNotExistsInRepositoryErrorCode);
    public static readonly string AggregationRootShouldNotExistsInRepositoryMessage = nameof(AggregationRootShouldNotExistsInRepositoryMessage);
    public static readonly NotificationType AggregationRootShouldNotExistsInRepositoryNotificationType = NotificationType.Error;
}

public abstract class ServiceBase<TAggregationRoot>
    : ServiceBase,
    IService<TAggregationRoot>
    where TAggregationRoot : IAggregationRoot
{
    // Properties
    protected INotificationPublisher NotificationPublisher { get; }
    protected IDomainEventPublisher DomainEventPublisher { get; }
    protected IAdapter Adapter { get; }
    protected IRepository<TAggregationRoot> Repository { get; }

    // Constructors
    protected ServiceBase(
        INotificationPublisher notificationPublisher,
        IDomainEventPublisher domainEventPublisher,
        IAdapter adapter,
        IRepository<TAggregationRoot> repository
    )
    {
        NotificationPublisher = notificationPublisher;
        DomainEventPublisher = domainEventPublisher;
        Adapter = adapter;
        Repository = repository;
    }

    // Protected Methods
    protected async Task<bool> ValidateDomainEntityAndSendNotificationsAsync(IDomainEntity domainEntityBase, CancellationToken cancellationToken)
    {
        foreach (var validationMessage in domainEntityBase.ValidationInfo.ValidationMessageCollection)
            await NotificationPublisher.PublishNotificationAsync(Adapter.Adapt<ValidationMessage, Notification>(validationMessage), cancellationToken);

        return domainEntityBase.ValidationInfo.IsValid;
    }
    protected async Task<bool> CheckIfAggregationRootExistInRepositoryAsync(
        Func<TAggregationRoot> getAggregationRootFunction, 
        CancellationToken cancellationToken
    )
    {
        var exists = getAggregationRootFunction() != null;

        if (!exists)
            await NotificationPublisher.PublishNotificationAsync(
                new Notification(
                    AggregationRootShouldExistsInRepositoryNotificationType,
                    AggregationRootShouldExistsInRepositoryErrorCode,
                    AggregationRootShouldExistsInRepositoryMessage,
                    notificationCollection: Enumerable.Empty<Notification>()
                ),
                cancellationToken
            );

        return exists;
    }
    protected async Task<bool> CheckIfAggregationRootNotExistInRepositoryAsync(
        Func<TAggregationRoot> getAggregationRootFunction,
        CancellationToken cancellationToken)
    {
        var exists = getAggregationRootFunction() != null;

        if (exists)
            await NotificationPublisher.PublishNotificationAsync(
                new Notification(
                    AggregationRootShouldNotExistsInRepositoryNotificationType,
                    AggregationRootShouldNotExistsInRepositoryErrorCode,
                    AggregationRootShouldNotExistsInRepositoryMessage,
                    notificationCollection: Enumerable.Empty<Notification>()
                ),
                cancellationToken
            );

        return exists;
    }
}