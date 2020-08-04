using System;
using Elwark.People.Abstractions;

namespace Elwark.Account.Web.Services.IdentityService.Model
{
    public class IdentityModel
    {
        public IdentityId IdentityId { get; set; }

        public Identification Identification { get; set; } = null!;

        public Notification Notification { get; set; } = null!;

        public DateTimeOffset? ConfirmedAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}