﻿using Domain.BoundedContexts.UserAccountManagement.ValueObjects;
using SharedKernel;
using System;

namespace Domain.BoundedContexts.UserAccountManagement.Events
{
    public class CustomerAccountCreatedEvent : DomainEvent
    {
        public EmailAddress Email { get; private set; }
        public HashedPassword Password { get; private set; }
        public UserFullName Name { get; private set; }

        private CustomerAccountCreatedEvent()
        { }

        public CustomerAccountCreatedEvent(Guid userId, EmailAddress email, HashedPassword password, UserFullName name)
        {
            AggregateId = userId;
            Email = email;
            Password = password;
            Name = name;
        }
    }
}
