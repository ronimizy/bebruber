﻿using System.Security.Authentication;
using Bebruber.Application.Requests.Accounts;
using Bebruber.DataAccess;
using Bebruber.Identity.Tools;
using Bebruber.Utility.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Bebruber.Application.Handlers.Accounts;

public class LoginCommandHandler : IRequestHandler<Login.Command, Login.Response>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public LoginCommandHandler(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IJwtTokenGenerator tokenGenerator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<Login.Response> Handle(Login.Command request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
            throw new AuthenticationException("Wrong email or password");

        var roles = await _userManager.GetRolesAsync(user);

        Console.WriteLine(roles.Count);

        roles.ForEach(Console.WriteLine);
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (result.Succeeded)
            return new Login.Response(_tokenGenerator.CreateToken(user));

        throw new AuthenticationException("Wrong email or password");
    }
}