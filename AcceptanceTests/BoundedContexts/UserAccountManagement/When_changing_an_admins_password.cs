﻿using AcceptanceTests.Framework;
using Application.BoundedContexts.UserAccountManagement.Commands;
using Application.Results;
using FluentAssertions;
using Domain.BoundedContexts.UserAccountManagement.Aggregates;
using Domain.BoundedContexts.UserAccountManagement.Events;
using Domain.BoundedContexts.UserAccountManagement.ValueObjects;
using MediatR;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AcceptanceTests.BoundedContexts.UserAccountManagement
{
    public class When_changing_an_admins_password : Specification<Admin, AdminAccountChangePasswordCommand, CommandResult>
    {
        private readonly Guid _UserId = Guid.NewGuid();
        private readonly string _CurrentPassword = "123456";
        private readonly string _NewPlainPassword = "654321";

        protected override IRequestHandler<AdminAccountChangePasswordCommand, CommandResult> CommandHandler()
            => new AdminAccountChangePasswordCommandHandler(
                MockEventSourcingRepository.Object,
                MockIdentityRepository.Object
            );

        protected override ICollection<IDomainEvent> GivenEvents()
            => new List<IDomainEvent>()
            {
                new AdminAccountCreatedEvent
                (
                    userId: _UserId,
                    email: (EmailAddress)EmailAddress.Create("test@user.com").ValueObject,
                    password: (HashedPassword)HashedPassword.Create(_CurrentPassword).ValueObject,
                    name: (UserFullName)UserFullName.Create("Test", "User").ValueObject
                )
            };

        protected override AdminAccountChangePasswordCommand When()
            => new AdminAccountChangePasswordCommand
            {
                UserId = _UserId,
                CurrentPassword = _CurrentPassword,
                NewPassword = _NewPlainPassword,
                ExpectedVersion = 0
            };

        [Then]
        public void Then_a_AdminAccountChangedPasswordEvent_will_be_published()
        {
            PublishedEvents.Last().As<AdminAccountChangedPasswordEvent>().AggregateId.Should().Be(_UserId);
            PublishedEvents.Last().As<AdminAccountChangedPasswordEvent>().NewPassword.ToString().Should().NotBeNullOrEmpty();
            PublishedEvents.Last().As<AdminAccountChangedPasswordEvent>().NewPassword.ToString().Should().NotBe(_NewPlainPassword);
            PublishedEvents.Last().As<AdminAccountChangedPasswordEvent>().NewPassword.ToString().Should().NotBe(_CurrentPassword);
        }
    }
}
