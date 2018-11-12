using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

// ReSharper disable once CheckNamespace

namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    public class MailAddressPhoneNumber : MailAddress
    {
        public MailAddressPhoneNumber(string address, string phoneNumber) : base(address)
        {
            PhoneNumber = phoneNumber;
        }

        public MailAddressPhoneNumber(string address, string displayName, string phoneNumber)
            : base(address, displayName)
        {
            PhoneNumber = phoneNumber;
        }

        public MailAddressPhoneNumber(string address, string displayName, Encoding displayNameEncoding,
            string phoneNumber) : base(address, displayName, displayNameEncoding)
        {
            PhoneNumber = phoneNumber;
        }

        public string PhoneNumber { get; set; }
    }

    public static class MailAddressPhoneNumberExtensions
    {
        public static string ToHtml(this MailAddressPhoneNumber item)
        {
            return item == null
                ? String.Empty
                : String.Format(
                    "<span class=\"mail-address\">&quot;{0}&quot; &lt;<a href=\"mailto:{1}\">{1}</a>&gt;{2}</span>",
                    item.DisplayName, item.Address,
                    String.IsNullOrWhiteSpace(item.PhoneNumber) ? String.Empty : " (" + item.PhoneNumber + ")");
        }

        public static string ToHtml(this List<MailAddressPhoneNumber> items)
        {
            if (items == null) return string.Empty;
            var sb = new StringBuilder();
            foreach (var item in items)
            {
                sb.Append(item.ToHtml());
            }
            return sb.ToString();
        }
    }
}