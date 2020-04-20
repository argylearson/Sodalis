using System.Diagnostics.CodeAnalysis;

namespace SodalisCore.Services {
    public interface ICryptographyService {
        internal void CreatePasswordHash([DisallowNull] string password, out byte[] hash, out byte[] salt);
        internal bool VerifyPasswordHash([DisallowNull] string password, byte[] storedHash, byte[] storedSalt);
    }
}