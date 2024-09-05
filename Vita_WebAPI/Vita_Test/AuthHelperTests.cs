using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Vita_WebApi_API;
using Vita_WebAPI_IdentityAPI.Helpers;
using Vita_WebAPI_Services;

namespace Vita_Test;
[TestClass]
public class HasScopeHandlerTests
{
    private HasScopeHandler? _handler;
    private HasScopeRequirement? _requirement;

    [TestInitialize]
    public void Setup()
    {
        _handler = new HasScopeHandler();
        _requirement = new HasScopeRequirement("required_scope", "issuer");
    }


    [TestMethod]
    public async Task HandleRequirementAsync_UserDoesNotHaveRequiredScope_DoesNotSucceed()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim("scope", "other_scope", "issuer")
        }));
        var context = new AuthorizationHandlerContext(new[] { _requirement }!, user, null);

        // Act
        await _handler!.HandleAsync(context);

        // Assert
        Assert.IsFalse(context.HasSucceeded);
    }

    [TestMethod]
    public async Task HandleRequirementAsync_UserDoesNotHaveScopeClaim_DoesNotSucceed()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        var context = new AuthorizationHandlerContext(new[] { _requirement }!, user, null);

        // Act
        await _handler?.HandleAsync(context)!;

        // Assert
        Assert.IsFalse(context.HasSucceeded);
    }
}
