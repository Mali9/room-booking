using System.Security.Cryptography;
using RoomBooking.Core.Abstractions;
using RoomBooking.Core.Domain;

namespace RoomBooking.Core.Services;

/// <summary>PBKDF2 password hashing. Format: iterations.salt.hash (Base64).</summary>
public sealed class PasswordHasher
{
    private const int Iterations = 10_000;
    private const int SaltBytes = 16;
    private const int HashBytes = 32;

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltBytes);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashBytes);
        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public bool Verify(string password, string stored)
    {
        var parts = stored.Split('.');
        if (parts.Length != 3 || !int.TryParse(parts[0], out var iterations))
        {
            return false;
        }

        var salt = Convert.FromBase64String(parts[1]);
        var expected = Convert.FromBase64String(parts[2]);
        var actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expected.Length);
        return CryptographicOperations.FixedTimeEquals(actual, expected);
    }
}

/// <summary>Sign-in (BRD BR-19).</summary>
public sealed class AuthService(IUserRepository users, PasswordHasher hasher)
{
    public User? ValidateCredentials(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(password))
        {
            return null;
        }

        var user = users.GetByEmail(email.Trim());
        return user is not null && hasher.Verify(password, user.PasswordHash) ? user : null;
    }

    public User? GetUser(string userId) => users.GetById(userId);
}
