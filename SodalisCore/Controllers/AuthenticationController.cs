﻿using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SodalisCore.DataTransferObjects;
using SodalisCore.Services;

namespace SodalisCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseSodalisController
    {
        private readonly IAuthenticationService _userService;

        public AuthenticationController(IAuthenticationService userService) {
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> Login([FromBody] LoginDto credentials) {
            async Task<IActionResult> Action() {
                var token = await _userService.Login(credentials);
                return new ObjectResult(token) {StatusCode = 200};
            }

            var result = await ResultToResponseAsync(Action);
            return result;
        }

        [HttpPost]
        [Route("register")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> Register([FromBody] UserDto user) {
            async Task<IActionResult> Action() {
                var newUser = await _userService.Register(user);
                return new ObjectResult(newUser) {StatusCode = 201};
            }

            var result = await ResultToResponseAsync(Action);
            return result;
        }

    }
}