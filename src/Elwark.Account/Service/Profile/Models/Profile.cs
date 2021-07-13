using System;

namespace Elwark.Account.Service.Profile.Models
{
    public sealed record Profile(
        long Id,
        string Nickname,
        string? FirstName,
        string? LastName,
        string FullName,
        string Language,
        Gender Gender,
        DateTime? DateOfBirth,
        string? Bio,
        string Picture,
        Address Address,
        Timezone Timezone,
        Ban? Ban,
        bool IsPasswordAvailable,
        DateTime CreatedAt,
        Connection[] Connections
    );
}
