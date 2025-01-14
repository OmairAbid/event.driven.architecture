﻿using Application.Results;
using Authentication.Repository;
using EventSourcingRepository.Repository;
using Domain.BoundedContexts.UserAccountManagement.Aggregates;
using Domain.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.BoundedContexts.UserAccountManagement.Commands
{
    public class AdminAccountChangePasswordCommand : IRequest<CommandResult>
    {
        public long ExpectedVersion { get; set; }
        public Guid UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class AdminAccountChangePasswordCommandHandler : IRequestHandler<AdminAccountChangePasswordCommand, CommandResult>
    {
        private readonly IEventSourcingRepository<Admin> _repository;
        private readonly IAuthenticationRepository _authRepository;

        public AdminAccountChangePasswordCommandHandler(IEventSourcingRepository<Admin> repository, IAuthenticationRepository authRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
        }

        public async Task<CommandResult> Handle(AdminAccountChangePasswordCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var admin = await _repository.FindByIdAsync(command.UserId);
                if (admin is null)
                    return CommandResult.NotFound(command.UserId);

                if (admin.Version != command.ExpectedVersion)
                    return CommandResult.NotFound(command.UserId);

                if (admin.Password != command.CurrentPassword)
                    return CommandResult.BusinessFail("Invalid Password.");

                admin.ChangePassword(
                    currentPassword: await _authRepository.GenerateHashedPassword(command.UserId, command.CurrentPassword),
                    newPassword: await _authRepository.GenerateHashedPassword(command.UserId, command.NewPassword)
                );

                await _repository.SaveAsync(admin);
                return CommandResult.Success();
            }
            catch (DomainException ex)
            {
                return CommandResult.BusinessFail(ex.Message);
            }
        }
    }
}
