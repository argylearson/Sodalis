using System;
using System.Net;

namespace SodalisExceptions.Exceptions {
    public class NotFoundException : BaseSodalisException {
        public NotFoundException(string message, Exception innerException = null) : base(message, innerException) { }
        public override HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.NotFound;
        public override ErrorMessage ClientMessage { get; set; } = new ErrorMessage("No object was found for the given id.");
    }
}