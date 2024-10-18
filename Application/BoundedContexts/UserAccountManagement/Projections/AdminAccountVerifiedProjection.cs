using Application.BoundedContexts.UserAccountManagement.QueryObjects;
using Domain.BoundedContexts.UserAccountManagement.Events;
using MediatR;
using QueryRepository.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.BoundedContexts.UserAccountManagement.Projections
{
    public class AdminAccountVerifiedProjection : INotificationHandler<AdminAccountVerifiedEvent>
    {
        private readonly IProjectionRepository<AdminInfo> _repository;

        public AdminAccountVerifiedProjection(IProjectionRepository<AdminInfo> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(AdminAccountVerifiedEvent @event, CancellationToken cancellationToken)
        {
            var customer = await _repository.FindByIdAsync(@event.AggregateId);
            if (customer == null)
            {
                await _repository.InsertAsync(new AdminInfo
                {
                    Id = @event.AggregateId,
                    Email = @event.Email.Value,
                    FirstName = @event.Name.First,
                    LastName = @event.Name.Last,
                    Version = @event.AggregateVersion
                });
            }
            else
            {
                customer.Email = @event.Email.Value;
                customer.Version = @event.AggregateVersion;

                await _repository.UpdateAsync(customer);
            }
        }
    }
}
