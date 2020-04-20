using System.Threading.Tasks;
using SodalisCore.DataTransferObjects;

namespace SodalisCore.Repositories {
    public interface IAuthenticationRepository {
        internal Task<UserDto> Register(UserDto user);
        internal Task<UserDto> Login(LoginDto credentials);
    }
}