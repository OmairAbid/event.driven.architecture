using Domain.BoundedContexts.UserAccountManagement.Aggregates;
using Domain.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Results;
using Authentication.Repository;
using EventSourcingRepository.Repository;

namespace Application.BoundedContexts.UserAccountManagement.Commands
{
    public class CustomerAccountChangePasswordCommand : IRequest<CommandResult>
    {
        public long ExpectedVersion { get; set; }
        public Guid UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class CustomerAccountChangePasswordCommandHandler : IRequestHandler<CustomerAccountChangePasswordCommand, CommandResult>
    {
        private readonly IEventSourcingRepository<Customer> _repository;
        private readonly IAuthenticationRepository _authRepository;

        public CustomerAccountChangePasswordCommandHandler(IEventSourcingRepository<Customer> repository, IAuthenticationRepository authRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
        }

        public async Task<CommandResult> Handle(CustomerAccountChangePasswordCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _repository.FindByIdAsync(command.UserId);
                if (customer is null)
                    return CommandResult.NotFound(command.UserId);

                if (customer.Version != command.ExpectedVersion)
                    return CommandResult.NotFound(command.UserId);

                if (customer.Password != command.CurrentPassword)
                    return CommandResult.BusinessFail("Invalid Password.");

                customer.ChangePassword(
                    currentPassword: await _authRepository.GenerateHashedPassword(command.UserId, command.CurrentPassword),
                    newPassword: await _authRepository.GenerateHashedPassword(command.UserId, command.NewPassword)
                );

                await _repository.SaveAsync(customer);
                return CommandResult.Success();
            }
            catch (DomainException ex)
            {
                return CommandResult.BusinessFail(ex.Message);
            }
        }
    }
}
