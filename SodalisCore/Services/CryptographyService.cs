using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SodalisCore.Services {
    internal class CryptographyService : ICryptographyService {
        void ICryptographyService.CreatePasswordHash(string password, out byte[] hash, out byte[] salt) {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        bool ICryptographyService.VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt) {
            using var hmac = new HMACSHA512(storedSalt);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            //check if any there are any elements where the values don't match
            //    vvv This is because we DON'T want to find any
            return !hash.Where((t, i) => !t.Equals(storedHash[i])).Any();
        }
    }
}