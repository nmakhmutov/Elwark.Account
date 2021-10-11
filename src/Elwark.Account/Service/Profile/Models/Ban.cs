using System;

namespace Elwark.Account.Service.Profile.Models;

public sealed record Ban(string Reason, DateTime? ExpiredAt);