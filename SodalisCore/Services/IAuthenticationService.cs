using System.Threading.Tasks;
using SodalisCore.DataTransferObjects;

namespace SodalisCore.Services {
    public interface IAuthenticationService {
        internal Task<UserDto> Register(UserDto user);
        internal Task<TokenDto> Login(LoginDto credentials);
    }
}
