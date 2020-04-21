using System.Threading.Tasks;
using SodalisCore.DataTransferObjects;
using SodalisCore.Services;
using SodalisDatabase;
using SodalisDatabase.ContextExtensions;
using SodalisDatabase.Entities;
using SodalisExceptions;
using SodalisExceptions.Exceptions;

namespace SodalisCore.Repositories {
    public class AuthenticationRepository : IAuthenticationRepository{
        private readonly SodalisContext _sodalisContext;
        private readonly ICryptographyService _cryptographyService;

        public AuthenticationRepository(SodalisContext sodalisContext, ICryptographyService cryptographyService) {
            _sodalisContext = sodalisContext;
            _cryptographyService = cryptographyService;
        }

        async Task<UserDto> IAuthenticationRepository.Register(UserDto user) {
            _cryptographyService.CreatePasswordHash(user.Password, out var hash, out var salt);

            var dbEntry = new User() {
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PasswordHash = hash,
                PasswordSalt = salt
            };
            dbEntry = await _sodalisContext.CreateUser(dbEntry);

            user = new UserDto {
                Id = dbEntry.Id,
                EmailAddress = dbEntry.EmailAddress,
                FirstName = dbEntry.FirstName,
                LastName = dbEntry.LastName
            };

            return user;
        }

        async Task<UserDto> IAuthenticationRepository.Login(LoginDto credentials) {
            try {
                var user = await _sodalisContext.GetUserByEmailAddress(credentials.EmailAddress);

                if (user == null)
                    throw new UnauthenticatedException($"No user found for ${credentials.EmailAddress}");
                if (user.IsDeleted)
                    throw new UnauthenticatedException($"A login attempt was made for user {user.Id}, but user is inactive.");
                if (!_cryptographyService.VerifyPasswordHash(credentials.Password, user.PasswordHash, user.PasswordSalt))
                    throw new UnauthenticatedException($"A login attempt was made for user {user.Id}, but password was incorrect.");

                return new UserDto {
                    Id = user.Id,
                    EmailAddress = user.EmailAddress,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
            }
            catch (BaseSodalisException ex) {
                ex.ClientMessage.Message = "No account found for the provided credentials.";
                throw;
            }
        }
    }
}