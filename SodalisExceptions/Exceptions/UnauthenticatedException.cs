using System;
using System.Net;

namespace SodalisExceptions.Exceptions {
    public class UnauthenticatedException : BaseSodalisException{
        public UnauthenticatedException(string message, Exception innerException = null) : base(message, innerException) { }
        public override HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.Unauthorized;
        public override ErrorMessage ClientMessage { get; set; } = new ErrorMessage("A valid authorization token is required to access this resource.");
    }
}