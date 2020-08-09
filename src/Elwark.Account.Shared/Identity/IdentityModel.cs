using System;
using Elwark.People.Abstractions;
using Newtonsoft.Json;

namespace Elwark.Account.Shared.Identity
{
    public class IdentityModel
    {
        public IdentityId IdentityId { get; set; }

        public Identification Identification { get; set; } = null!;

        public Notification Notification { get; set; } = null!;

        public DateTimeOffset? ConfirmedAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public bool IsConfirmed => ConfirmedAt.HasValue;
        
        public bool IsLoading { get; set; }
        
        public bool IsConfirmationCodeSent { get; set; }
    }
}