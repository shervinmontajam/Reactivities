using Application.Errors;
using Application.Interfaces;
using Application.Validation;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.User
{
    public class Register
    {
        public class Command : IRequest<User>
        {
            public string DisplayName { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.DisplayName);
                RuleFor(p => p.Username);
                RuleFor(p => p.Email).EmailAddress();
                RuleFor(p => p.Password).Password();
            }
        }

        public class Handler : IRequestHandler<Command, User>
        {
            private readonly DataContext _dataContext;
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(DataContext dataContext, UserManager<AppUser> userManager, IJwtGenerator jwtGenerator)
            {
                _dataContext = dataContext;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
            }


            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                await ValidateRegisterUser(request, cancellationToken);

                var user = new AppUser
                {
                    DisplayName = request.DisplayName,
                    Email = request.Email,
                    UserName = request.Username
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                    return new User
                    {
                        DisplayName = user.DisplayName,
                        Token = _jwtGenerator.CreateToken(user),
                        Username = user.UserName,
                        Image = null
                    };

                throw new Exception("Something went wrong creating a user");
            }

            private async Task ValidateRegisterUser(Command request, CancellationToken cancellationToken)
            {
                if (await _dataContext.Users.Where(p => p.Email == request.Email).AnyAsync(cancellationToken: cancellationToken))
                    throw new RestException(HttpStatusCode.BadRequest, new {Email = "Email Already Exists"});

                if (await _dataContext.Users.Where(p => p.UserName == request.Username)
                    .AnyAsync(cancellationToken: cancellationToken))
                    throw new RestException(HttpStatusCode.BadRequest, new {Username = "Username Already Exists"});
            }

        }
    }
}
