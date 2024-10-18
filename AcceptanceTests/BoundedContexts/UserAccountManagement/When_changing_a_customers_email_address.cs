using AcceptanceTests.Framework;
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
    public class When_changing_a_customers_email_address : Specification<Customer, CustomerAccountChangeEmailCommand, CommandResult>
    {
        private readonly Guid _UserId = Guid.NewGuid();
        private readonly string _NewEmail = "changed@user.com";
        private readonly string _Firstname = "Test";
        private readonly string _Lastname = "User";

        protected override IRequestHandler<CustomerAccountChangeEmailCommand, CommandResult> CommandHandler()
            => new CustomerAccountChangeEmailCommandHandler(
                MockEventSourcingRepository.Object,
                MockIdentityRepository.Object
            );

        protected override ICollection<IDomainEvent> GivenEvents()
            => new List<IDomainEvent>()
            {
                new CustomerAccountCreatedEvent
                (
                    userId: _UserId,
                    email: (EmailAddress)EmailAddress.Create("test@user.com").ValueObject,
                    password: (HashedPassword)HashedPassword.Create("123456").ValueObject,
                    name: (UserFullName)UserFullName.Create("Test", "User").ValueObject
                )
            };

        protected override CustomerAccountChangeEmailCommand When()
            => new CustomerAccountChangeEmailCommand
            {
                UserId = _UserId,
                NewEmail = _NewEmail,
                ExpectedVersion = 0
            };

        [Then]
        public void Then_a_CustomerAccountChangedEmailEvent_will_be_published()
        {
            PublishedEvents.Last().As<CustomerAccountChangedEmailEvent>().AggregateId.Should().Be(_UserId);
            PublishedEvents.Last().As<CustomerAccountChangedEmailEvent>().Name.ToString().Should().Be($"{_Firstname} {_Lastname}");
            PublishedEvents.Last().As<CustomerAccountChangedEmailEvent>().Email.ToString().Should().Be(_NewEmail);
        }
    }
}
