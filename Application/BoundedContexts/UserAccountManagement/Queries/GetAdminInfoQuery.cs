﻿using Application.BoundedContexts.UserAccountManagement.QueryObjects;
using MediatR;
using QueryRepository.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.BoundedContexts.UserAccountManagement.Queries
{
    public class GetAdminInfoQuery : IRequest<AdminInfo>
    {
        public Guid Id { get; private set; }
        public GetAdminInfoQuery(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class GetAdminInfoQueryHandler : IRequestHandler<GetAdminInfoQuery, AdminInfo>
    {
        private readonly IQueryRepository<AdminInfo> _repository;

        public GetAdminInfoQueryHandler(IQueryRepository<AdminInfo> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<AdminInfo> Handle(GetAdminInfoQuery request, CancellationToken cancellationToken)
        {
            return await _repository.FindByIdAsync(request.Id);
        }
    }
}
