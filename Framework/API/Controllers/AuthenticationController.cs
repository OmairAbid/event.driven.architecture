﻿using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using API.DTOs;
using Application.BoundedContexts.UserAccountManagement.Commands;
using Authentication.Configuration;
using Authentication.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IAuthenticationRepository _authRepository;

        public AuthenticationController(IMediator mediator, IAuthenticationRepository authRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> VerifyEmailAddress(Guid userId, string token)
        {
            var verificationResult = await _authRepository.IsValidEmailVerificationToken(userId, token);
            if (verificationResult is false)
                return BadRequest();

            if (await _authRepository.UserHasRole(userId, FixedRoles.AdminRole))
            {
                await _mediator.Send(new AdminAccountVerifyCommand
                {
                    UserId = userId
                });

                return Ok();
            }

            if (await _authRepository.UserHasRole(userId, FixedRoles.CustomerRole))
            {
                await _mediator.Send(new CustomerAccountVerifyCommand
                {
                    UserId = userId
                });

                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentialsDTO dto)
        {
            try
            {
                return Ok(await _authRepository.GenerateJWT(
                    email: dto.Username,
                    password: dto.Password
                ));
            }
            catch (AuthenticationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
