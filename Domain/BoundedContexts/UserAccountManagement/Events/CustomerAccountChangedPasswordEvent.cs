using Domain.BoundedContexts.UserAccountManagement.ValueObjects;
using SharedKernel;
using System;

namespace Domain.BoundedContexts.UserAccountManagement.Events
{
    public class CustomerAccountChangedPasswordEvent : DomainEvent
    {
        public HashedPassword NewPassword { get; protected set; }

        private CustomerAccountChangedPasswordEvent()
        { }

        public CustomerAccountChangedPasswordEvent(Guid userId, HashedPassword newPassword)
        {
            AggregateId = userId;
            NewPassword = newPassword;
        }
    }
}
