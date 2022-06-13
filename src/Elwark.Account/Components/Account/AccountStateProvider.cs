using Elwark.Account.Gateways.Profile;
using Elwark.Account.Gateways.Profile.Models;

namespace Elwark.Account.Components.Account;

internal sealed class AccountStateProvider
{
    private readonly IAccountClient _client;
    private AccountState _state;

    public AccountStateProvider(IAccountClient client)
    {
        _client = client;
        _state = new AccountState();
    }

    public event Action<AccountState> StateChanged = _ => { };

    public AccountState GetAccount() =>
        _state;

    public async Task InitAsync()
    {
        var account = await _client.GetAsync();
        if (account.IsSuccess)
            Update(account.Data);
    }

    public void Update(Gateways.Profile.Models.Account account)
    {
        _state = new AccountState
        {
            IsInitialized = true,
            Nickname = account.Nickname,
            FirstName = account.FirstName,
            LastName = account.LastName,
            PreferNickname = account.PreferNickname,
            Language = account.Language,
            Picture = account.Picture,
            CountryCode = account.CountryCode,
            CreatedAt = account.CreatedAt,
            DateFormat = account.DateFormat,
            FullName = account.FullName,
            TimeFormat = account.TimeFormat,
            TimeZone = account.TimeZone,
            WeekStart = account.WeekStart,
            Emails = account.Emails,
            Connections = account.Connections
        };

        StateChanged.Invoke(_state);
    }

    public void Add(Email email)
    {
        _state = _state with
        {
            Emails = new List<Email>(_state.Emails)
                .Append(email)
                .ToArray()
        };

        StateChanged.Invoke(_state);
    }

    public void Update(Email email)
    {
        var list = new List<Email>(_state.Emails);
        var index = list.FindIndex(x => x.Value == email.Value);
        if(index < 0)
            return;

        list[index] = email;
        
        _state = _state with
        {
            Emails = list.ToArray()
        };

        StateChanged.Invoke(_state);
    }

    public void Update(Email[] emails)
    {
        _state = _state with
        {
            Emails = emails
        };

        StateChanged.Invoke(_state);
    }
    
    public void Delete(Email email)
    {
        _state = _state with
        {
            Emails = _state.Emails
                .Where(x => x.Value != email.Value)
                .ToArray()
        };

        StateChanged.Invoke(_state);
    }

    public void Delete(Connection connection)
    {
        _state = _state with
        {
            Connections = _state.Connections
                .Where(x => (x.Type == connection.Type && x.Identity == connection.Identity) == false)
                .ToArray()
        };
        
        StateChanged.Invoke(_state);
    }
}
