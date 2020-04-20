using System;
using System.Net;

namespace SodalisExceptions.Exceptions {
    public class BadRequestException : BaseSodalisException {
        public BadRequestException(string message, Exception innerException = null) : base(message, innerException) { }

        public override HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.BadRequest;
        public override ErrorMessage ClientMessage { get; set; } = new ErrorMessage("There was an error with the request. Please look for errors and try again.");
    }
}