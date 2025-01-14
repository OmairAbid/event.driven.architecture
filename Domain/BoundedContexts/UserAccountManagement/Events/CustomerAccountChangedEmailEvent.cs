﻿using Domain.BoundedContexts.UserAccountManagement.ValueObjects;
using SharedKernel;
using System;

namespace Domain.BoundedContexts.UserAccountManagement.Events
{
    public class CustomerAccountChangedEmailEvent : DomainEvent
    {
        public EmailAddress Email { get; protected set; }
        public UserFullName Name { get; protected set; }

        private CustomerAccountChangedEmailEvent()
        { }

        public CustomerAccountChangedEmailEvent(Guid userId, EmailAddress email, UserFullName name)
        {
            AggregateId = userId;
            Email = email;
            Name = name;
        }
    }
}
