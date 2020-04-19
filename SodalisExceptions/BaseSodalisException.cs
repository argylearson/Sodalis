using System;
using System.Net;

namespace SodalisExceptions {
    public abstract class BaseSodalisException : Exception {

        protected BaseSodalisException(string message, Exception innerException) : base(message, innerException) {

        }

        public abstract HttpStatusCode HttpStatusCode { get; }
        public int HttpCode => (int) HttpStatusCode;
        public abstract string ClientMessage { get; }
    }
}