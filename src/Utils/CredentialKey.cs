using System;

namespace Soenneker.Google.Credentials.Utils;

internal readonly struct CredentialKey : IEquatable<CredentialKey>
{
    private readonly string _fileName;
    private readonly string[] _scopes;

    public CredentialKey(string fileName, string[] scopes)
    {
        _fileName = fileName;
        _scopes = scopes;
    }

    public bool Equals(CredentialKey other)
    {
        if (!StringComparer.Ordinal.Equals(_fileName, other._fileName))
            return false;

        string[]? a = _scopes;
        string[]? b = other._scopes;

        if (ReferenceEquals(a, b))
            return true;

        if (a is null || b is null || a.Length != b.Length)
            return false;

        for (int i = 0; i < a.Length; i++)
        {
            if (!StringComparer.Ordinal.Equals(a[i], b[i]))
                return false;
        }

        return true;
    }

    public override bool Equals(object? obj) => obj is CredentialKey other && Equals(other);

    public override int GetHashCode()
    {
        var hc = new HashCode();
        hc.Add(_fileName, StringComparer.Ordinal);

        string[]? s = _scopes;
        if (s != null)
        {
            for (int i = 0; i < s.Length; i++)
                hc.Add(s[i], StringComparer.Ordinal);
        }

        return hc.ToHashCode();
    }
}