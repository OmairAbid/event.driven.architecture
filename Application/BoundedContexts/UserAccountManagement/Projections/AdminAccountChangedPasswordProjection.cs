﻿using Application.BoundedContexts.UserAccountManagement.QueryObjects;
using Authentication.Repository;
using Domain.BoundedContexts.UserAccountManagement.Events;
using MediatR;
using QueryRepository.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.BoundedContexts.UserAccountManagement.Projections
{
    public class AdminAccountChangedPasswordProjection : INotificationHandler<AdminAccountChangedPasswordEvent>
    {
        private readonly IAuthenticationRepository _authRepository;
        private readonly IProjectionRepository<AdminInfo> _repository;

        public AdminAccountChangedPasswordProjection(IAuthenticationRepository authRepository, IProjectionRepository<AdminInfo> repository)
        {
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(AdminAccountChangedPasswordEvent @event, CancellationToken cancellationToken)
        {
            await _authRepository.ChangePassword(
                userId: @event.AggregateId,
                hashedPassword: @event.NewPassword
            );

            var admin = await _repository.FindByIdAsync(@event.AggregateId);
            if (admin is not null)
            {
                admin.Version = @event.AggregateVersion;
                await _repository.UpdateAsync(admin);
            }
            else
            {

            }
        }
    }
}
