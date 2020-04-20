using System;
using System.Net;

namespace SodalisExceptions {
    public abstract class BaseSodalisException : Exception {

        protected BaseSodalisException(string message, Exception innerException = null) : base(message, innerException) {

        }

        public abstract HttpStatusCode HttpStatusCode { get; set; }
        public int HttpCode => (int) HttpStatusCode;
        public abstract ErrorMessage ClientMessage { get; set; }
    }
}