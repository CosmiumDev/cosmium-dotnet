using System;
using Cosmium.EmbeddedServer.Constants;
using Cosmium.EmbeddedServer.Contracts;

namespace Cosmium.EmbeddedServer.Exceptions
{
    public class CosmiumServerException : Exception
    {
        public ResponseType ResponseType { get; }

        public CosmiumServerException(ResponseType responseType, string message) : base(message)
        {
            this.ResponseType = responseType;
        }

        public CosmiumServerException(long responseCode, string message) : base(message)
        {
            this.ResponseType = (ResponseType)responseCode;
        }

        public string InformationalMessage => $"{this.ResponseType}: {ResponseMessages.GetMessage(ResponseType)}";
    }
}
