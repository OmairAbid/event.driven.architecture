﻿using MediatR;

namespace SharedKernel
{
    public abstract class EventSourcingNotification : INotification
    {
        public bool IsReplayingEvent { get; set; }
        public bool ShouldSerializeIsReplayingEvent() => false;
    }
}
