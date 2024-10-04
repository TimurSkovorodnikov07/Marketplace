using Microsoft.AspNetCore.Authorization;

public class AgeRequirement(int minAge) : IAuthorizationRequirement
{
    public int MinAge { get; init; } = minAge;
}