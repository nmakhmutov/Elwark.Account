using System;
using Elwark.Account.Service.Profile.Models;

namespace Elwark.Account.States
{
    public class ProfileStateProvider
    {
        public event Action ProfileStateChanged = () => { };

        public Profile Profile { get; private set; } = default!;

        public void Update(Profile profile)
        {
            Profile = profile;
            ProfileStateChanged.Invoke();
        }
    }
}