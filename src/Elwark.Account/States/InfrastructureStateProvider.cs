using System;
using System.Collections.Generic;

namespace Elwark.Account.States
{
    public class InfrastructureStateProvider
    {
        public event Action InfrastructureStateChanged = () => { };

        public InfrastructureState Infrastructure { get; private set; }
            = new(new Dictionary<string, string>(), new Dictionary<string, string>());

        public void Update(InfrastructureState infrastructure)
        {
            Infrastructure = infrastructure;
            InfrastructureStateChanged.Invoke();
        }
    }
}