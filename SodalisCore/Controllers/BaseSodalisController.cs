using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SodalisCore.DataTransferObjects;
using SodalisExceptions;

namespace SodalisCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseSodalisController : ControllerBase
    {
        protected static async Task<IActionResult> ResultToResponseAsync(Func<Task<IActionResult>> action) {

            IActionResult result;
            try {
                result = await action();
            } catch (BaseSodalisException ex) {
                result = new ObjectResult(new ErrorMessageDto(ex.ClientMessage)) { StatusCode = ex.HttpCode };
            } catch (Exception ex) {
                result = new ObjectResult(new ErrorMessageDto("An unexpected error occurred.")) { StatusCode = 500 };
            }

            return result;
        }
    }
}