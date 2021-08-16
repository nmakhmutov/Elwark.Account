using System;

namespace Elwark.Account.Service.Profile.Models
{
    public sealed record Profile(
        long Id,
        string Nickname,
        bool PreferNickname,
        string? FirstName,
        string? LastName,
        string FullName,
        string Language,
        Gender Gender,
        DateTime? DateOfBirth,
        string? Bio,
        string Picture,
        Address Address,
        TimeInfo TimeInfo,
        Ban? Ban,
        bool IsPasswordAvailable,
        DateTime CreatedAt,
        Connection[] Connections
    );
}
