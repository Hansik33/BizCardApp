using BizCardApp.Enums.ValidationResults.Optional;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace BizCardApp.Validators.Optional;

public static partial class EmailValidator
{
    private const int MaxEmailLength = 254;
    private const int MaxLocalLength = 64;

    [GeneratedRegex(@"^[A-Za-z0-9._+\-]+$")]
    private static partial Regex LocalAllowedRegex();

    [GeneratedRegex(@"^[A-Za-z0-9\-]+$")]
    private static partial Regex DomainLabelRegex();

    public static EmailValidationResult Validate(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return EmailValidationResult.NotProvided;

        var value = email.Trim();

        if (value.Length > MaxEmailLength)
            return EmailValidationResult.TooLong;

        int atIndex = value.IndexOf('@');
        if (atIndex <= 0 || atIndex == value.Length - 1)
            return EmailValidationResult.InvalidFormat;

        var localPart = value[..atIndex];
        var domainPart = value[(atIndex + 1)..];

        if (localPart.Length > MaxLocalLength)
            return EmailValidationResult.TooLong;

        if (!LocalAllowedRegex().IsMatch(localPart))
            return EmailValidationResult.InvalidCharacters;

        if (localPart.StartsWith('.') || localPart.EndsWith('.') || localPart.Contains(".."))
            return EmailValidationResult.InvalidFormat;

        if (!domainPart.Contains('.'))
            return EmailValidationResult.InvalidFormat;

        var labels = domainPart.Split('.');
        if (labels.Any(string.IsNullOrEmpty))
            return EmailValidationResult.InvalidFormat;

        foreach (var label in labels)
        {
            if (label.Length is < 1 or > 63)
                return EmailValidationResult.InvalidFormat;

            if (!DomainLabelRegex().IsMatch(label))
                return EmailValidationResult.InvalidCharacters;

            if (label.StartsWith('-') || label.EndsWith('-'))
                return EmailValidationResult.InvalidFormat;
        }

        var tld = labels[^1];
        if (tld.Length < 2)
            return EmailValidationResult.InvalidFormat;

        return EmailValidationResult.Valid;
    }

    public static string? Normalize(string? email, bool normalizeIdnDomain = true)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        var trimmed = email.Trim();
        int atIndex = trimmed.IndexOf('@');
        if (atIndex <= 0 || atIndex == trimmed.Length - 1)
            return trimmed;

        var local = trimmed[..atIndex];
        var domain = trimmed[(atIndex + 1)..];

        domain = domain.ToLowerInvariant();

        if (normalizeIdnDomain && domain.Any(character => character > 127))
        {
            try
            {
                var idn = new IdnMapping();
                domain = string.Join('.',
                    domain.Split('.')
                          .Select(part => idn.GetAscii(part)));
            }
            catch
            {
            }
        }

        return $"{local}@{domain}";
    }
}