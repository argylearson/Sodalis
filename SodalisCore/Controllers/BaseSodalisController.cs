using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SodalisExceptions;

namespace SodalisCore.Controllers
{
    public abstract class BaseSodalisController : ControllerBase
    {
        protected static async Task<IActionResult> ResultToResponseAsync(Func<Task<IActionResult>> action) {

            IActionResult result;
            try {
                result = await action();
            } catch (BaseSodalisException ex) {
                result = new ObjectResult(ex.ClientMessage) { StatusCode = ex.HttpCode };
            } catch (Exception) {
                result = new ObjectResult(new ErrorMessage("An unexpected error occurred.")) { StatusCode = 500 };
            }

            return result;
        }
    }
}