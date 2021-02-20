using Elwark.Account.Service.Profile;
using Elwark.Account.Service.Profile.Models;

namespace Elwark.Account
{
    public delegate void ProfileStateChangedHandler();
    
    public class ProfileStateProvider
    {
        public event ProfileStateChangedHandler ProfileStateChanged = () => { };
        
        public Profile Profile { get; private set; } = default!;

        public void Update(Profile profile)
        {
            Profile = profile;
            StateChanged();
        }
        private void StateChanged()
        {
            ProfileStateChanged.Invoke();
        }
    }
}