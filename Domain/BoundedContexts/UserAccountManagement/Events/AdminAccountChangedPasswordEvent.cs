using Domain.BoundedContexts.UserAccountManagement.ValueObjects;
using SharedKernel;
using System;

namespace Domain.BoundedContexts.UserAccountManagement.Events
{
    public class AdminAccountChangedPasswordEvent : DomainEvent
    {
        public HashedPassword NewPassword { get; protected set; }

        private AdminAccountChangedPasswordEvent()
        { }

        public AdminAccountChangedPasswordEvent(Guid userId, HashedPassword newPassword)
        {
            AggregateId = userId;
            NewPassword = newPassword;
        }
    }
}
