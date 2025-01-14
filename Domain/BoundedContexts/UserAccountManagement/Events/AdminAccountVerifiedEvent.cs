﻿using Domain.BoundedContexts.UserAccountManagement.ValueObjects;
using SharedKernel;
using System;

namespace Domain.BoundedContexts.UserAccountManagement.Events
{
    public class AdminAccountVerifiedEvent : DomainEvent
    {
        public UserFullName Name { get; private set; }
        public EmailAddress Email { get; private set; }
        public bool Verified { get; private set; }

        private AdminAccountVerifiedEvent()
        { }

        internal AdminAccountVerifiedEvent(Guid userId, EmailAddress email, UserFullName name, bool verified)
        {
            AggregateId = userId;
            Name = name;
            Email = email;
            Verified = verified;
        }
    }
}
