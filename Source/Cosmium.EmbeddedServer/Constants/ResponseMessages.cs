using Cosmium.EmbeddedServer.Contracts;

namespace Cosmium.EmbeddedServer.Constants
{
    internal static class ResponseMessages
    {
        private const string Success = "Success";
        private const string Unknown = "Unknown";
        private const string FailedToParseConfiguration = "Failed to parse configuration";
        private const string FailedToLoadState = "Failed to load state";
        private const string FailedToParseRequest = "Failed to parse request";
        private const string ServerInstanceAlreadyExists = "Server instance already exists";
        private const string ServerInstanceNotFound = "Server instance not found";
        private const string FailedToStartServer = "Failed to start server";
        private const string RepositoryNotFound = "Repository not found";
        private const string RepositoryConflict = "Repository conflict";
        private const string RepositoryBadRequest = "Repository bad request";

        internal static string GetMessage(ResponseType responseType)
        {
            return responseType switch
            {
                ResponseType.Success => Success,
                ResponseType.Unknown => Unknown,
                ResponseType.FailedToParseConfiguration => FailedToParseConfiguration,
                ResponseType.FailedToLoadState => FailedToLoadState,
                ResponseType.FailedToParseRequest => FailedToParseRequest,
                ResponseType.ServerInstanceAlreadyExists => ServerInstanceAlreadyExists,
                ResponseType.ServerInstanceNotFound => ServerInstanceNotFound,
                ResponseType.FailedToStartServer => FailedToStartServer,
                ResponseType.RepositoryNotFound => RepositoryNotFound,
                ResponseType.RepositoryConflict => RepositoryConflict,
                ResponseType.RepositoryBadRequest => RepositoryBadRequest,
                _ => Unknown,
            };
        }
    }
}
