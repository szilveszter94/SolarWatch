using Microsoft.AspNetCore.Authorization;

namespace SolarWatchUnitTests.IntegrationTests.Services;

public class FakeAuthorizationHandler : AuthorizationHandler<IAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
    {
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
