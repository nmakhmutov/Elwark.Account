using System;
using Elwark.Account.Service.Country;
using Elwark.Account.Service.Timezone;

namespace Elwark.Account.States
{
    public class InfrastructureStateProvider
    {
        public event Action InfrastructureStateChanged = () => { };

        public InfrastructureState State { get; private set; } =
            new(Array.Empty<Country>(), Array.Empty<Timezone>());

        public void Update(Country[] countries)
        {
            State = State with {Countries = countries};
            InfrastructureStateChanged.Invoke();
        }

        public void Update(Timezone[] timezones)
        {
            State = State with {Timezones = timezones};
            InfrastructureStateChanged.Invoke();
        }
    }
}
