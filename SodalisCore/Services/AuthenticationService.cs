using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SodalisCore.DataTransferObjects;
using SodalisCore.Repositories;
using SodalisExceptions.Exceptions;

namespace SodalisCore.Services {
    public class AuthenticationService : IAuthenticationService{

        private readonly IAuthenticationRepository _repository;
        private readonly IClaimService _claimService;

        public AuthenticationService(IAuthenticationRepository repository, IClaimService claimService) {
            _repository = repository;
            _claimService = claimService;
        }

        Task<UserDto> IAuthenticationService.Register(UserDto user) {
            ValidateUser(user);
            ValidateEmail(user.EmailAddress);
            ValidatePassword(user.Password);

            return _repository.Register(user);
        }

        async Task<TokenDto> IAuthenticationService.Login(LoginDto credentials) {
            ValidateEmail(credentials.EmailAddress);
            ValidatePassword(credentials.Password);

            var user = await _repository.Login(credentials);
            var token = _claimService.CreateToken(user);

            return new TokenDto {
                Id = user.Id,
                Token = token
            };
        }

        private static void ValidateUser(UserDto user) {
            if (string.IsNullOrWhiteSpace(user.FirstName))
                throw new BadRequestException("User provided an empty first name") {
                    ClientMessage = {Message = "Please provide a valid first name and try again."}
                };

            if (string.IsNullOrWhiteSpace(user.LastName))
                throw new BadRequestException("User provided an empty last name") {
                    ClientMessage = { Message = "Please provide a valid last name and try again." }
                };
        }

        private static void ValidateEmail(string emailAddress) {
            if (string.IsNullOrWhiteSpace(emailAddress))
                throw new BadRequestException("User provided an empty email address") {
                    ClientMessage = { Message = "Please provide a valid email address and try again." }
                };
            try {
                //normalize the domain
                static string DomainMapper(Match match) {
                    //use this to convert unicode domain names
                    var internetDomainName = new IdnMapping();
                    //extract and process domain name or throw ArgumentException if invalid
                    var domainName = internetDomainName.GetAscii(match.Groups[2].Value);
                    return match.Groups[1].Value + domainName;
                }

                emailAddress = Regex.Replace(emailAddress, @"(@)(.+)$", DomainMapper, RegexOptions.None,
                    TimeSpan.FromMilliseconds(200));

                _ = Regex.IsMatch(emailAddress,
                    @"^(?("")("".+?(?<!\\)""@)" + //if the username starts with a quote, it should end with a quote
                    @"|(([0-9a-z]" + //quotes aside, look for alphanumeric characters
                    @"((\.(?!\.))" + //periods are ok, but not two consecutive periods
                    @"|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)" + //these characters are also fine
                    @"(?<=[0-9a-z])@))" + //any additional alphanumerics immediately before the '@'
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])" + //if there are brackets in the domain name, the value between them should be an ip address
                    @"|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+" + //in the domain name, look for alphanumerics and hyphens followed by a period 1 or more times
                    @"[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", //allow up to 22 characters for the top-level domain
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException ex) {
                throw new BadRequestException("Email regex timed out", ex) {
                    ClientMessage = {Message = "There was an error processing the provided email address. Please examine the provided value and try again."}
                };
            }
            catch (ArgumentException ex) {
                throw new BadRequestException("Invalid email address", ex) {
                    ClientMessage = { Message = "The provided email address was invalid. Please provide a valid value and try again." }
                };
            }
        }

        private static void ValidatePassword(string password) {
            if (string.IsNullOrWhiteSpace(password))
                throw new BadRequestException("User provided an empty password") {
                    ClientMessage = { Message = "Please provide a password and try again." }
                };
            //TODO add more password validation
        }
    }
}