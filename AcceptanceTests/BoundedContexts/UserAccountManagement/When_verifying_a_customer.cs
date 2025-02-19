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
    public class When_verifying_a_customer : Specification<Customer, CustomerAccountVerifyCommand, CommandResult>
    {
        private readonly Guid _UserId = Guid.NewGuid();
        private readonly string _Email = "test@user.com";
        private readonly string _PlainPassword = "123456";
        private readonly string _Firstname = "Test";
        private readonly string _Lastname = "User";

        protected override IRequestHandler<CustomerAccountVerifyCommand, CommandResult> CommandHandler()
            => new CustomerAccountVerifyCommandHandler(MockEventSourcingRepository.Object);

        protected override ICollection<IDomainEvent> GivenEvents()
            => new List<IDomainEvent>()
            {
                new CustomerAccountCreatedEvent
                (
                    userId: _UserId,
                    email: (EmailAddress)EmailAddress.Create(_Email).ValueObject,
                    password: (HashedPassword)HashedPassword.Create(_PlainPassword).ValueObject,
                    name: (UserFullName)UserFullName.Create(_Firstname, _Lastname).ValueObject
                )
            };

        protected override CustomerAccountVerifyCommand When()
            => new CustomerAccountVerifyCommand
            {
                UserId = _UserId
            };

        [Then]
        public void Then_a_CustomerAccountVerifiedEvent_will_be_published()
        {
            PublishedEvents.Last().As<CustomerAccountVerifiedEvent>().AggregateId.Should().Be(_UserId);
            PublishedEvents.Last().As<CustomerAccountVerifiedEvent>().Name.ToString().Should().Be($"{_Firstname} {_Lastname}");
            PublishedEvents.Last().As<CustomerAccountVerifiedEvent>().Email.ToString().Should().Be(_Email);
            PublishedEvents.Last().As<CustomerAccountVerifiedEvent>().Verified.Should().BeTrue();
        }
    }
}
