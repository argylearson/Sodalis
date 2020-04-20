using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SodalisExceptions;
using SodalisExceptions.Exceptions;

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

        protected static bool ParsePagingParameters(string stringPageNumber, string stringPageSize, 
            out int pageNumber, out int pageSize, int maxPageSize = 25) {
            if (string.IsNullOrWhiteSpace(stringPageNumber) && string.IsNullOrWhiteSpace(stringPageSize)) {
                pageNumber = 0;
                pageSize = 0;
                return false;
            }

            if (string.IsNullOrWhiteSpace(stringPageNumber))
                pageNumber = 1;
            else if(!int.TryParse(stringPageNumber, out pageNumber) || pageNumber < 1)
                throw new BadRequestException("Page number was invalid") {
                    ClientMessage = new ErrorMessage("The provided page number was invalid. Please correct it and try again.")
                };

            if (string.IsNullOrWhiteSpace(stringPageSize))
                pageSize = 25;
            else if(!int.TryParse(stringPageSize, out pageSize) || pageSize < 1 || pageSize > maxPageSize)
                throw new BadRequestException("Page size was invalid") {
                    ClientMessage = new ErrorMessage("The provided page size was invalid. Please correct it and try again.")
                };

            return true;
        }

        protected static bool ParsePagingParameters(int? pageNumber, int? pageSize,
            out int outPageNumber, out int outPageSize, int maxPageSize = 25) {
            if (!pageNumber.HasValue && !pageSize.HasValue) {
                outPageNumber = 0;
                outPageSize = 0;
                return false;
            }
            outPageNumber = pageNumber ?? 1;
            if (pageNumber < 1)
                throw new BadRequestException("Page number was invalid") {
                    ClientMessage =
                        new ErrorMessage("The provided page number was invalid. Please correct it and try again.")
                };
            outPageSize = pageSize ?? 25;
            if (pageSize < 1 || pageSize > maxPageSize)
                throw new BadRequestException("Page size was invalid") {
                    ClientMessage =
                        new ErrorMessage("The provided page size was invalid. Please correct it and try again.")
                };

            return true;
        }
    }
}