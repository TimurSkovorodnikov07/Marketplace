using Microsoft.AspNetCore.Authorization;

namespace server_app.Presentation.Policy;

public class AgeRequirement(int minAge) : IAuthorizationRequirement
{
    public int MinAge { get; init; } = minAge;
}