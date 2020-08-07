using System;
using Elwark.People.Abstractions;

namespace Elwark.Account.Web.Models
{
    public class AttachLinkModel
    {
        public AttachLinkModel(IdentificationType type, Uri link)
        {
            Link = link;
            Type = type;
        }

        public Uri Link { get; }

        public IdentificationType Type { get; }
    }
}