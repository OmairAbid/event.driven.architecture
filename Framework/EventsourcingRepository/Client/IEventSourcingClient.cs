﻿using SharedKernel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSourcingRepository.Client
{
    public interface IEventSourcingClient
    {
        Task<(long Version, IEnumerable<IDomainEvent> Events)> ReadEventsAsync(Guid aggregateId);
        Task AppendEventAsync(IDomainEvent @event);
    }
}
