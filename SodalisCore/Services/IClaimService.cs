using SodalisCore.DataTransferObjects;

namespace SodalisCore.Services {
    public interface IClaimService {
        internal string CreateToken(UserDto user);
    }
}