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
    public class When_changing_an_admins_email_address : Specification<Admin, AdminAccountChangeEmailCommand, CommandResult>
    {
        private readonly Guid _UserId = Guid.NewGuid();
        private readonly string _NewEmail = "changed@user.com";
        private readonly string _Firstname = "Test";
        private readonly string _Lastname = "User";

        protected override IRequestHandler<AdminAccountChangeEmailCommand, CommandResult> CommandHandler()
            => new AdminAccountChangeEmailCommandHandler(
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
                    password: (HashedPassword)HashedPassword.Create("123456").ValueObject,
                    name: (UserFullName)UserFullName.Create("Test", "User").ValueObject
                )
            };

        protected override AdminAccountChangeEmailCommand When()
            => new AdminAccountChangeEmailCommand
            {
                UserId = _UserId,
                NewEmail = _NewEmail,
                ExpectedVersion = 0
            };

        [Then]
        public void Then_a_AdminAccountChangedEmailEvent_will_be_published()
        {
            PublishedEvents.Last().As<AdminAccountChangedEmailEvent>().AggregateId.Should().Be(_UserId);
            PublishedEvents.Last().As<AdminAccountChangedEmailEvent>().Name.ToString().Should().Be($"{_Firstname} {_Lastname}");
            PublishedEvents.Last().As<AdminAccountChangedEmailEvent>().Email.ToString().Should().Be(_NewEmail);
        }
    }
}
